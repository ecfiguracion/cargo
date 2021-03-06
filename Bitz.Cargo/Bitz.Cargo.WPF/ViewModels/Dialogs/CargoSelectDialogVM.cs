﻿using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Dialogs
{
  public class CargoSelectDialogVM : DialogViewModelBase<BaseItemInfos>
  {
    public override void Initialise()
    {
      base.Initialise();
      this.Refresh();
    }

    #region Properties

    public BaseItemInfos.Criteria _Criteria = new BaseItemInfos.Criteria();
    public BaseItemInfos.Criteria Criteria
    {
      get { return _Criteria; }
      set
      {
        _Criteria = value;
        OnPropertyChanged("Criteria");
      }
    }
    #endregion

    #region Methods

    public override void Refresh()
    {
      BaseItemInfos.Get(this.Criteria, (o, e) =>
      {
        if (e.Error != null) throw e.Error;
        this.Model = e.Object;
      });
    }

    #endregion
  }
}
