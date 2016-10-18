﻿using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class PaymentsVM : GridViewModelBase<PaymentInfos>
  {
    #region Initialise

    public PaymentsVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new PaymentInfos.Criteria();

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