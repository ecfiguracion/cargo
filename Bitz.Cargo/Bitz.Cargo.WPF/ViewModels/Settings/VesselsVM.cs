using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Contacts.Infos;
using Bitz.Core.Constants;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class VesselsVM : GridViewModelBase<ContactInfos>
  {
    #region Initialise

    public VesselsVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new ContactInfos.Criteria();
      criteria.ContactType = BitzConstants.ContactTypes.Vessel.Id;

      this.Criteria = criteria;
     
      this.Refresh();
    }

    #endregion

    #region Properties

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion

  }
}
