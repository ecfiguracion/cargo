using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class UnitOfMeasuresVM : GridViewModelBase<UnitOfMeasureInfos>
  {
    #region Initialise

    public UnitOfMeasuresVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new UnitOfMeasureInfos.Criteria();
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
