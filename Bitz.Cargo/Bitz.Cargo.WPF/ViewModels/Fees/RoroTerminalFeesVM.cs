using Bitz.Cargo.Business.Fees.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Fees
{
  public class RoroTerminalFeesVM : GridViewModelBase<VehicleFeeInfos>
  {
    #region Initialise

    public RoroTerminalFeesVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new VehicleFeeInfos.Criteria();
      criteria.Status = CargoConstants.RoroStatus.All.Id;

      this.Criteria = criteria;

      this.Refresh();
    }

    #endregion

    #region Properties

    public List<CoreConstants.IdValue> Statuses
    {
      get
      {
        return CargoConstants.RoroStatus.Items;
      }
    }

    #endregion

    #region Commands

    public override void CommandRemoveExecute(object parameter)
    {
      if (this.SelectedItem != null)
      {
        var item = this.SelectedItem as VehicleFeeInfo;
        if (item.Status.Id != CargoConstants.BillStatus.Draft.Id)
        {
          NavigationManager.ShowMessage("Remove", "Only DRAFT entries are allowed to be removed.", System.Windows.MessageBoxButton.OK);
        }
        else
        {
          base.CommandRemoveExecute(parameter);
        }
      }
    }

    #endregion

    #region Methods

    #endregion

  }
}
