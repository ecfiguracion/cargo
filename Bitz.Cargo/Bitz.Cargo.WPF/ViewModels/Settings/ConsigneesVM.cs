﻿using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.Data;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class ConsigneesVM : GridViewModelBase<ConsigneeInfos>
  {
    #region Initialise

    public ConsigneesVM() { }

    public override void Initialise()
    {
      base.Initialise();

      this.Criteria = new ConsigneeInfos.Criteria();
     
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
