using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class ForeignsVM : GridViewModelBase<BillInfos>
  {
    #region Initialise

    public ForeignsVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new BillInfos.Criteria();
      criteria.BillType = CargoConstants.BillingType.Foreign.Id;
      criteria.Status = CargoConstants.BillStatus.Draft.Id;

      this.Criteria = criteria;
     
      this.Refresh();
    }

    #endregion

    #region Properties

    public List<CoreConstants.IdValue> Statuses
    {
      get
      {
        return CargoConstants.BillStatus.Items;
      }
    }

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion

  }
}
