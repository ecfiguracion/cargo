using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class RorosVM : GridViewModelBase<BillingItemInfos>
  {
    #region Initialise

    public RorosVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new BillingItemInfos.Criteria();
      criteria.BillingItemType = CargoConstants.BillingType.Roro.Id;

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
