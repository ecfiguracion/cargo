using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.ViewModel;
using Csla.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.ViewModels.Dialogs
{
  public class BillSelectDialogVM : DialogViewModelBase<BillPaymentInfos>
  {
    public void Initialise(int consignee)
    {
      base.Initialise();

      this.Criteria.Consignee = consignee;

      this.Refresh();
    }

    #region Properties

    public BillPaymentInfos.Criteria _Criteria = new BillPaymentInfos.Criteria();
    public BillPaymentInfos.Criteria Criteria
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
      MobileList<int> billtypes = new MobileList<int>();
      billtypes.Add(CargoConstants.BillingType.Foreign.Id);
      billtypes.Add(CargoConstants.BillingType.Domestic.Id);

      this.Criteria.BillTypes = billtypes;

      MobileList<int> statuses = new MobileList<int>();
      statuses.Add(CargoConstants.BillStatus.Draft.Id);
      statuses.Add(CargoConstants.BillStatus.PartiallyPaid.Id);
      this.Criteria.Statuses = statuses;

      BillPaymentInfos.Get(this.Criteria, (o, e) =>
      {
        if (e.Error != null) throw e.Error;
        this.Model = e.Object;
      });
    }

    #endregion
  }
}
