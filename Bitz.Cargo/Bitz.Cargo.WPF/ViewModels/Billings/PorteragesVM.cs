﻿using Bitz.Cargo.Business.Billing.Infos;
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
  public class PorteragesVM : GridViewModelBase<BillInfos>
  {
    #region Initialise

    public PorteragesVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new BillInfos.Criteria();
      criteria.BillType = CargoConstants.BillingType.Porterage.Id;
      criteria.Status = CargoConstants.BillStatus.All.Id;
      criteria.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      criteria.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

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
