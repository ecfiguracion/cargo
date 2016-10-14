using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.ViewModel;
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
      BillPaymentInfos.Get(this.Criteria, (o, e) =>
      {
        if (e.Error != null) throw e.Error;
        this.Model = e.Object;
      });
    }

    #endregion
  }
}
