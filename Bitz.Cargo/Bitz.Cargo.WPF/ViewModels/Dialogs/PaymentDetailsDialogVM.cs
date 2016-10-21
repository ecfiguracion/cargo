using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Constants;
using Bitz.Core.ViewModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bitz.Cargo.ViewModels.Dialogs
{
  public class PaymentDetailsDialogVM : DialogViewModelBase<PaymentDetail>
  {
    public void Initialise(PaymentDetail paymentdetail)
    {
      base.Initialise();

      this.CommandOk = new DelegateCommand<object>(CommandOkExecute);

      this.Model = paymentdetail;
    }

    #region Properties

    #region PaymentTypes

    public List<CoreConstants.IdValue> PaymentTypes
    {
      get
      {
        return CargoConstants.PaymentTypes.Items;
      }
    }


    #endregion

    #endregion

    #region Methods

    #endregion

    #region Commands

    #region CommandOk
    public ICommand CommandOk
    {
      get;
      private set;
    }

    public virtual void CommandOkExecute(object parameter)
    {
      this.Model.ApplyEdit();
      this.View.Close();
    }
    #endregion

    public override void CommandCancelExecute(object parameter)
    {
      this.Model.CancelEdit();
      base.CommandCancelExecute(parameter);
    }
    #endregion
  }
}
