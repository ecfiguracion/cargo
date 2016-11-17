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
  public class MooringsVM : GridViewModelBase<BillInfos>
  {
    #region Initialise

    public MooringsVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new BillInfos.Criteria();
      criteria.BillType = CargoConstants.BillingType.Mooring.Id;
      criteria.Status = CargoConstants.BillStatus.All.Id;
      criteria.MooringType = CargoConstants.MooringType.All.Id;

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

    public List<CoreConstants.IdValue> MooringTypes
    {
      get
      {
        return CargoConstants.MooringType.Items;
      }
    }

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion

  }
}
