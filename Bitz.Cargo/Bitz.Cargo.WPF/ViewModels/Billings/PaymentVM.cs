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
using Csla.Rules;
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
  public class PaymentVM : PageViewModelBase<Payment>
  {
    #region Initialise

    public PaymentVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

      this.CommandSelectConsignee = new DelegateCommand<object>(CommandSelectConsigneeExecute);
      this.CommandSelectBill = new DelegateCommand<object>(CommandSelectBillExecute);
      this.CommandRemoveBill = new DelegateCommand<object>(CommandRemoveBillExecute);
      this.CommandPaymentDetails = new DelegateCommand<object>(CommandPaymentDetailsExecute);
      this.CommandApprove = new DelegateCommand<object>(CommandApproveExecute);
    }

    #endregion

    #region Internal Events

    protected override void OnModelChanged(Payment oldValue, Payment newValue)
    {
      base.OnModelChanged(oldValue, newValue);
      newValue.ChildChanged += newValue_ChildChanged;
      OnPropertyChanged("TotalAmountPaid");

      this.IsReadOnly = this.Model.Status.Id == CargoConstants.PaymentStatus.Approved.Id;
      OnPropertyChanged("CanSave");
    }

    void newValue_ChildChanged(object sender, Csla.Core.ChildChangedEventArgs e)
    {
      if (e.PropertyChangedArgs != null && e.PropertyChangedArgs.PropertyName == "AmountPaid")
      {
        this.Model.TotalAmountPaid = this.Model.PaymentDetails.Sum(x => x.AmountPaid);
      }
    }

    #endregion

    #region Properties

    #region SelectedItem

    public PaymentDetail SelectedItem { get; set; }

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
      if (!this.IsReadOnly)
      {
        EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Subscribe(SelectedConsigneeResult);
        NavigationManager.Show(UserInterfaces.Bitz.ContactSelectDialog, new object[] { BitzConstants.ContactTypes.Consignee.Id });
      }
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
      if (this.IsReadOnly) return;
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
          payment.PaymentType = CargoConstants.PaymentTypes.Cash.Id;
          payment.RefDate = new Csla.SmartDate(DateTime.Now);
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
      if (this.IsReadOnly) return;
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

    #region CommandPaymentDetails
    public ICommand CommandPaymentDetails
    {
      get;
      private set;
    }

    public void CommandPaymentDetailsExecute(object parameter)
    {
      if (SelectedItem != null)
      {
        NavigationManager.Show(UserInterfaces.Cargo.PaymentDetailsDialog, new object[] { this.SelectedItem });
      }
    }

    #endregion

    #region CommandApprove
    public ICommand CommandApprove
    {
      get;
      private set;
    }

    public void CommandApproveExecute(object parameter)
    {
      if (!this.IsReadOnly)
      {
        var result = NavigationManager.ShowMessage("Approve", "This will mark this payment as approved. Approve payment(s) are not editable. \n\nAre you sure you want to proceed?", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
          this.Model.Status = CargoConstants.PaymentStatus.Approved;
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
