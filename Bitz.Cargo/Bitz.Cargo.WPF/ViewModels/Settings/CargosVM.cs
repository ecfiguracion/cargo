using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Constants;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class CargosVM : GridViewModelBase<ItemInfos>
  {
    #region Initialise

    public CargosVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new ItemInfos.Criteria();
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
