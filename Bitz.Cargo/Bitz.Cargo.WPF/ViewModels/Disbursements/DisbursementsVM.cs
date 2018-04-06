using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Disbursements.Infos;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Disbursements
{
  public class DisbursementsVM : GridViewModelBase<DisbursementInfos>
  {
    #region Initialise

    public DisbursementsVM() { }

    public override void Initialise()
    {
      base.Initialise();

      var criteria = new DisbursementInfos.Criteria();
      criteria.Status = CargoConstants.BillStatus.All.Id;
      criteria.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      criteria.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month));
      this.Criteria = criteria;

      VoucherTypeInfos.Get(new VoucherTypeInfos.Criteria() {} , (o,e) => {
        if (e.Error != null) throw e.Error;
        this.Types = e.Object;
        this.Refresh();
      });
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

    private VoucherTypeInfos _Types;
    public VoucherTypeInfos Types
    {
      get { return _Types; }
      set { _Types = value; }
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
