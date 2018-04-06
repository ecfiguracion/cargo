using Bitz.Business.Contacts.Infos;
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

namespace Bitz.Cargo.ViewModels.Fees
{
  public class RoroTerminalFeeVM : PageViewModelBase<VehicleFee>
  {
    #region Initialise

    public RoroTerminalFeeVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

      this.CommandAddItem = new DelegateCommand<object>(CommandAddItemExecute);
      this.CommandRemoveItem = new DelegateCommand<object>(CommandRemoveItemExecute);
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

    protected override void OnModelChanged(VehicleFee oldValue, VehicleFee newValue)
    {
      base.OnModelChanged(oldValue, newValue);
      this.Model.ChildChanged += Model_ChildChanged;
      this.Model.PropertyChanged += Model_PropertyChanged;
      OnPropertyChanged("TotalServiceFees");
      OnPropertyChanged("TotalVAT");
      OnPropertyChanged("Total");
    }

    void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "PPATotal")
      {
        OnPropertyChanged("TotalServiceFees");
        OnPropertyChanged("TotalVAT");
        OnPropertyChanged("Total");
      }
    }

    void Model_ChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      if (e.PropertyChangedArgs != null)
      {
        if (e.PropertyChangedArgs.PropertyName == "VehicleType")
        {
          var fee = this.VehicleTypes.SingleOrDefault(x => x.Id == this.SelectedItem.VehicleType);
          if (fee != null)
          {
            this.SelectedItem.Fee = fee.Fee;
          }
        }

        if (e.PropertyChangedArgs.PropertyName == "Fee")
        {
          OnPropertyChanged("TotalServiceFees");
          OnPropertyChanged("TotalVAT");
          OnPropertyChanged("Total");
        }
      }
    }

    protected override void OnSaved()
    {
      base.OnSaved();
      this.IsReadOnly = this.Model.Status.Id != CargoConstants.StandardStatus.Draft.Id;
    }

    #endregion

    #region Properties

    #region VehicleTypes

    private VehicleTypeInfos _VehicleTypes;
    public VehicleTypeInfos VehicleTypes
    {
      get { return _VehicleTypes; }
      set
      {
        _VehicleTypes = value;
        OnPropertyChanged("VehicleTypes");
      }
    }

    #endregion

    #region SelectedItem

    public VehicleFeeItem SelectedItem { get; set; }

    #endregion

    #region TotalServiceFees

    public decimal TotalServiceFees
    {
      get
      {
        if (this.Model == null) return 0;
        if (this.Model.VehicleFeeItems == null) return 0;

        return this.Model.VehicleFeeItems.Sum(x => (decimal)x.Fee);
      }
    }

    #endregion

    #region TotalVAT

    public decimal TotalVAT
    {
      get
      {
        if (this.Model == null) return 0;

        return this.Model.PPATotal - this.TotalServiceFees;
      }
    }

    #endregion

    #region Total

    public decimal Total
    {
      get
      {
        if (this.Model == null) return 0;

        return this.Model.PPATotal;
      }
    }

    #endregion

    #endregion

    #region Commands

    #region CommandAddItem
    public ICommand CommandAddItem
    {
      get;
      private set;
    }

    public void CommandAddItemExecute(object parameter)
    {
      var item = VehicleFeeItem.New();
      if (this.Model.VehicleFeeItems.Count > 0)
      {
        int invoiceId = 0;
        var invoiceNo = this.Model.VehicleFeeItems[0].InvoiceNo;
        int.TryParse(invoiceNo, out invoiceId);
        if (invoiceId > 0)
        {
          invoiceId = invoiceId + this.Model.VehicleFeeItems.Count;
          item.InvoiceNo = invoiceId.ToString();
        }
      }

      this.Model.VehicleFeeItems.Add(item);
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
          this.Model.VehicleFeeItems.Remove(this.SelectedItem);
        }
      }
    }

    #endregion

    #region CommandPrint

    public override void CommandPrintExecute(object parameter)
    {
      if (!this.Model.IsNew)
      {
        ReportHelper.Print(Reports.Cargo.RPT0010, this.Model.Id);
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
        this.Model.Status = CargoConstants.StandardStatus.Cancelled;
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

      VehicleTypeInfos.Get(new VehicleTypeInfos.Criteria(), (o, e) =>
      {
        if (e.Error != null) throw e.Error;

        this.VehicleTypes = e.Object;
        datasourcescount += 1;

        if (datasourcescount == datasourcestotal)
          resultCallback(true);
      });
    }

    #endregion

    #endregion
  }
}
