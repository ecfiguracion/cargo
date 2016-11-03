using Bitz.Business.Contacts;
using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Constants;
using Bitz.Core.Events;
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
  public class ConsigneeDialogVM : DialogViewModelBase<Consignee>
  {
    public override void Initialise()
    {
      this.CommandOk = new DelegateCommand<object>(CommandOkExecute);

      Consignee.New((o, e) =>
      {
        if (e.Error != null) throw e.Error;
        this.Model = e.Object;
      });
    }

    #region Properties

    #endregion

    #region Events

    protected override void OnSaved()
    {
      base.OnSaved();
      BaseContactInfos.Get(new BaseContactInfos.Criteria() { Id = this.Model.Contact.Id }, (oo, ee) =>
      {
        if (ee.Error != null) throw ee.Error;

        BaseContactInfo contact = null;
        if (ee.Object.Count > 0)
        {
          contact = ee.Object[0];
          EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Publish(contact);
        }
        this.View.Close();
      });
    }

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
      BeginSave();
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
