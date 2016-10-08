using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
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

namespace Bitz.Cargo.ViewModels.Billings
{
  public class PaymentVM : PageViewModelBase<Foreign>
  {
    #region Initialise

    public PaymentVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

    }

    public override void Initialise(int? id)
    {
      //this.LoadLookupReferences((isloaded) =>
      //{
      //  if (isloaded)
      //  {
      //    base.Initialise(id);
      //    //LoadConfiguredItemRates();
      //  }
      //});
    }

    #endregion

    #region Internal Events

    #endregion

    #region Properties



    #endregion
  }
}
