using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class DomesticsVM : GridViewModelBase<BillInfos>
  {
    #region Initialise

    public DomesticsVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new BillInfos.Criteria();
      criteria.BillType = CargoConstants.BillingType.Domestic.Id;
      criteria.Status = CargoConstants.BillStatus.All.Id;

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

    public override void CommandRemoveExecute(object parameter)
    {
      if (this.SelectedItem != null)
      {
        var item = this.SelectedItem as BillInfo;
        if (item.Status.Id != CargoConstants.BillStatus.Draft.Id)
        {
          NavigationManager.ShowMessage("Remove", "Only DRAFT bills are allowed to be removed.", System.Windows.MessageBoxButton.OK);
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
