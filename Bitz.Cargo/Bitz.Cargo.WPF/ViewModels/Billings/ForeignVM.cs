﻿using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Events;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Bitz.Core.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class ForeignVM : PageViewModelBase<Foreign>
  {
    #region Initialise

    public ForeignVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

      this.CommandSelectConsignee = new DelegateCommand<object>(CommandSelectConsigneeExecute);
      this.CommandSelectVessel = new DelegateCommand<object>(CommandSelectVesselExecute);

      this.CommandAddItem = new DelegateCommand<object>(CommandAddItemExecute);
      this.CommandRemoveItem = new DelegateCommand<object>(CommandRemoveItemExecute);

      this.CommandOpenPayment = new DelegateCommand<object>(CommandOpenPaymentExecute);
    }

    public override void Initialise(int? id)
    {
      this.LoadLookupReferences((isloaded) =>
      {
        if (isloaded)
        {
          base.Initialise(id);
        }
      });
    }

    #endregion

    #region Internal Events

    protected override void OnModelChanged(Foreign oldValue, Foreign newValue)
    {
      base.OnModelChanged(oldValue, newValue);
      OnPropertyChanged("CanCancelDocument");
      this.IsReadOnly = this.Model.Status.Id != CargoConstants.BillStatus.Draft.Id;
      newValue.ChildChanged += newValue_ChildChanged;
      OnPropertyChanged("TotalAmountPaid");
    }

    protected override void OnSaved()
    {
      base.OnSaved();
      this.IsReadOnly = this.Model.Status.Id != CargoConstants.BillStatus.Draft.Id;
    }

    public override void CommandUndoExecute(object parameter)
    {
      base.CommandUndoExecute(parameter);
      OnPropertyChanged("CanCancelDocument");
    }

    void newValue_ChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      if (e.PropertyChangedArgs != null)
      {
        if (this.Model.BillItems.IsValid && (e.PropertyChangedArgs.PropertyName == "UnitCount" || e.PropertyChangedArgs.PropertyName == "Uom" ||
          e.PropertyChangedArgs.PropertyName == "WeightUsed" || e.PropertyChangedArgs.PropertyName == "IsTaxable"))
        {
          this.ComputeTotalBill();
        }
      }
    }

    #endregion

    #region Properties

    #region UnitOfMeasures

    private UnitOfMeasureInfos _UnitOfMeasures;
    public UnitOfMeasureInfos UnitOfMeasures
    {
      get { return _UnitOfMeasures; }
      set
      {
        _UnitOfMeasures = value;
        OnPropertyChanged("UnitOfMeasures");
      }
    }

    #endregion

    #region SelectedItem

    public BillItem SelectedItem { get; set; }

    #endregion

    #region WeightTypes

    public List<CoreConstants.IdValue> WeightRates
    {
      get
      {
        return CargoConstants.WeightRates.Items;
      }
    }

    #endregion

    #region SelectedPayment

    public PaymentInfo SelectedPayment { get; set; }

    #endregion

    #region TotalAmountPaid

    public decimal TotalAmountPaid
    {
      get
      {
        if (this.Model == null) return 0;
        if (this.Model.Payments == null) return 0;

        return this.Model.Payments.Sum(x => x.AmountPaid);
      }
    }

    #endregion

    #region CanCancelDocument
    public override bool CanCancelDocument
    {
      get
      {
        if (this.Model == null) return false;
        return this.Model.Status.Id != CargoConstants.BillStatus.Draft.Id
          && this.Model.Status.Id != CargoConstants.BillStatus.Cancelled.Id
          && !this.Model.IsDirty;
      }
    }
    #endregion

    #endregion

    #region Commands

    #region CommandSelectConsignee
    public ICommand CommandSelectConsignee
    {
      get;
      private set;
    }

    public void CommandSelectConsigneeExecute(object parameter)
    {
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Subscribe(SelectedConsigneeResult);
      NavigationManager.Show(UserInterfaces.Bitz.ContactSelectDialog,new object[] { BitzConstants.ContactTypes.Consignee.Id } );
    }

    public void SelectedConsigneeResult(object payload)
    {
      var contact = payload as BaseContactInfo;
      if (contact != null)
      {
        this.Model.Consignee = contact;
        this.Model.BillingAddress = contact.Address;
      }
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Unsubscribe(SelectedConsigneeResult);
    }

    #endregion

    #region CommandSelectVessel
    public ICommand CommandSelectVessel
    {
      get;
      private set;
    }

    public void CommandSelectVesselExecute(object parameter)
    {
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Subscribe(SelectedVesselResult);
      NavigationManager.Show(UserInterfaces.Bitz.ContactSelectDialog, new object[] { BitzConstants.ContactTypes.Vessel.Id });
    }

    public void SelectedVesselResult(object payload)
    {
      var contact = payload as BaseContactInfo;
      if (contact != null)
      {
        this.Model.Vessel = contact;
      }
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Unsubscribe(SelectedVesselResult);
    }
    #endregion

    #region CommandAddItem
    public ICommand CommandAddItem
    {
      get;
      private set;
    }

    public void CommandAddItemExecute(object parameter)
    {
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Subscribe(SelectedCargoResult);
      NavigationManager.Show(UserInterfaces.Cargo.CargoSelectDialog);
    }

    public void SelectedCargoResult(object payload)
    {
      var item = payload as BaseItemInfo;
      if (item != null)
      {
        if (!this.Model.BillItems.Any(x => x.Cargo.Id == item.Id))
        {
          var billitem = BillItem.New();
          billitem.Cargo = item;
          billitem.WeightUsed = CargoConstants.WeightRates.MetricTons.Id;
          billitem.IsTaxable = true;
          this.Model.BillItems.Add(billitem);
        } else
        {
          NavigationManager.ShowMessage("Error","Selected cargo already exists on the list, please retry.", MessageBoxButton.OK);
        }
      }
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Unsubscribe(SelectedCargoResult);
    }
    #endregion

    #region CommandRemoveItem
    public ICommand CommandRemoveItem
    {
      get;
      private set;
    }

    public void CommandRemoveItemExecute(object parameter)
    {
      if (SelectedItem != null)
      {
        var result = NavigationManager.ShowMessage("Remove", "Are you sure you want to delete the selected record?", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
          this.Model.BillItems.Remove(this.SelectedItem);
          ComputeTotalBill();
        }
      }
    }

    #endregion

    #region CommandOpenPayment
    public ICommand CommandOpenPayment
    {
      get;
      private set;
    }

    public void CommandOpenPaymentExecute(object parameter)
    {
      if (SelectedPayment != null)
      {
        NavigationManager.Show(UserInterfaces.Cargo.Payment, new object[] { this.SelectedPayment.Id });
      }
    }

    #endregion

    #region CommandPrint

    public override void CommandPrintExecute(object parameter)
    {
      if (!this.Model.IsNew)
      {
        ReportHelper.Print(Reports.Cargo.RPT0001, this.Model.Id);
      }
    }
    #endregion

    #region CommandCancel
    public override void CommandCancelExecute(object parameter)
    {
      var result = NavigationManager.ShowMessage("Cancel", "Marking this document 'CANCELLED' will make this document uneditable. \n\nAre you sure you want to proceed?", MessageBoxButton.YesNo);
      if (result == MessageBoxResult.Yes)
      {
        this.IsReadOnly = false;
        this.Model.Status = CargoConstants.BillStatus.Cancelled;
        OnPropertyChanged("CanCancelDocument");
      }
    }
    #endregion

    #endregion

    #region Methods

    #region LoadLookupReferences

    private void LoadLookupReferences(Action<bool> resultCallback)
    {
      var datasourcestotal = 1;
      var datasourcescount = 0;

      UnitOfMeasureInfos.Get(new UnitOfMeasureInfos.Criteria(), (o, e) =>
      {
        if (e.Error != null) throw e.Error;

        this.UnitOfMeasures = e.Object;
        datasourcescount += 1;

        if (datasourcescount == datasourcestotal)
          resultCallback(true);
      });
    }

    #endregion

    #region ComputeTotalBill

    private void ComputeTotalBill()
    {
      decimal stevedoringtotal = 0;
      decimal arrastretotal = 0;
      decimal ratetotal = 0;
      decimal wtaxtotal = 0;
      decimal totalbill = 0;
      decimal grandtotalbill = 0;
      foreach (var item in this.Model.BillItems)
      {
        if (item.QtyConversion == 0) continue;
        item.GetRate();
        var conversiontotal = (item.UnitCount * item.QtyConversion) / 1000;
        if (item.WeightUsed == CargoConstants.WeightRates.MetricTons.Id)
        {
          stevedoringtotal = Math.Round((decimal)conversiontotal * item.StevedoringRate, 2, MidpointRounding.AwayFromZero);
          arrastretotal = Math.Round((decimal)conversiontotal * item.ArrastreRate, 2, MidpointRounding.AwayFromZero);
        }
        else
        {
          stevedoringtotal = Math.Round((decimal)conversiontotal * item.StevedoringConst * item.StevedoringRate, 2, MidpointRounding.AwayFromZero);
          arrastretotal = Math.Round((decimal)conversiontotal * item.ArrastreConst * item.ArrastreRate, 2, MidpointRounding.AwayFromZero);
        }

        ratetotal = Math.Round(stevedoringtotal + arrastretotal, 2, MidpointRounding.AwayFromZero);
        if (item.PremiumRate > 0)
        {
          ratetotal += Math.Round(ratetotal * (item.PremiumRate / 100), 2, MidpointRounding.AwayFromZero);
        }

        totalbill = Math.Round(ratetotal * (decimal)1.12, 2, MidpointRounding.AwayFromZero);

        if (this.Model.WTaxRate > 0 && item.IsTaxable)
        {
          wtaxtotal = Math.Round(ratetotal * (this.Model.WTaxRate / 100), 2, MidpointRounding.AwayFromZero);
          totalbill -= wtaxtotal;
        }

        grandtotalbill += totalbill;
      }

      this.Model.TotalBill = Math.Round(grandtotalbill, 2, MidpointRounding.AwayFromZero);
    }
    #endregion

    #endregion
  }
}
