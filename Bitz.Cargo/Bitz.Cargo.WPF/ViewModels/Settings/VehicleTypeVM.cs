﻿using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Events;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Bitz.Core.ViewModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class VehicleTypeVM : PageViewModelBase<VehicleType>
  {
    #region Initialise

    public VehicleTypeVM()
    {
    }

    #endregion

    #region Internal Events

    #endregion

    #region Properties

    public override bool CanPrint
    {
      get
      {
        return false;
      }
    }

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion
  }
}
