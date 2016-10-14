using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Events;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Bitz.Core.ViewModel;
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
  public class PaymentVM : PageViewModelBase<Payment>
  {
    #region Initialise

    public PaymentVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

      this.CommandSelectConsignee = new DelegateCommand<object>(CommandSelectConsigneeExecute);
      this.CommandSelectBill = new DelegateCommand<object>(CommandSelectBillExecute);
      this.CommandRemoveBill = new DelegateCommand<object>(CommandRemoveBillExecute);
    }

    #endregion

    #region Internal Events

    protected override void OnModelChanged(Payment oldValue, Payment newValue)
    {
      base.OnModelChanged(oldValue, newValue);
      newValue.ChildChanged += newValue_ChildChanged;
      OnPropertyChanged("TotalAmountPaid");
    }

    void newValue_ChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      if (e.PropertyChangedArgs != null)
      {
        if (this.Model.PaymentDetails.IsValid)
        {
          OnPropertyChanged("TotalAmountPaid");
        }
      }
    }

    #endregion

    #region Properties

    #region SelectedItem

    public PaymentDetail SelectedItem { get; set; }

    #endregion

    #region TotalAmountPaid

    public decimal TotalAmountPaid
    {
      get
      {
        if (this.Model == null) return 0;

        return this.Model.PaymentDetails.Sum(x => (decimal)x.AmountPaid);
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
      NavigationManager.Show(UserInterfaces.Bitz.ContactSelectDialog, new object[] { BitzConstants.ContactTypes.Consignee.Id });
    }

    public void SelectedConsigneeResult(object payload)
    {
      var contact = payload as BaseContactInfo;
      if (contact != null)
      {
        this.Model.Consignee = contact;
      }
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Unsubscribe(SelectedConsigneeResult);
    }

    #endregion

    #region CommandSelectBill
    public ICommand CommandSelectBill
    {
      get;
      private set;
    }

    public void CommandSelectBillExecute(object parameter)
    {
      if (this.Model.Consignee != null)
      {
        EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Subscribe(SelectedSOAResult);
        NavigationManager.Show(UserInterfaces.Cargo.BillSelectDialog, new object[] { this.Model.Consignee.Id });
      }
      else
      {
        NavigationManager.ShowMessage("Error", "Please select first the consignee.", MessageBoxButton.OK);
      }
    }

    public void SelectedSOAResult(object payload)
    {
      var item = payload as BillPaymentInfo;
      if (item != null)
      {
        if (!this.Model.PaymentDetails.Any(x => x.Bill.Id == item.Id))
        {
          var payment = PaymentDetail.New();
          payment.Bill = item;
          payment.PartialPayment = item.AmountPaid;
          payment.AmountDue = item.AmountDue;
          this.Model.PaymentDetails.Add(payment);
        }
        else
        {
          NavigationManager.ShowMessage("Error", "Selected SOA already exists on the list, please retry.", MessageBoxButton.OK);
        }
      }
      EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Unsubscribe(SelectedSOAResult);
    }
    #endregion

    #region CommandRemoveBill
    public ICommand CommandRemoveBill
    {
      get;
      private set;
    }

    public void CommandRemoveBillExecute(object parameter)
    {
      if (SelectedItem != null)
      {
        var result = NavigationManager.ShowMessage("Remove", "Are you sure you want to remove the selected record?", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
          this.Model.PaymentDetails.Remove(this.SelectedItem);
        }
      }
    }

    #endregion

    #region CommandPrint

    public override void CommandPrintExecute(object parameter)
    {
      if (!this.Model.IsNew)
      {
        ReportHelper.Print(Reports.Cargo.StatementOfAccount, this.Model.Id);
      }
    }
    #endregion

    #endregion
  }
}
