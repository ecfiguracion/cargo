using Bitz.Core.Application;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitz.Core.Constants;
using Bitz.Core.Shell;

namespace Bitz.Cargo.ViewModels
{
  public class BillingMainVM : GridViewModelBase<object>
  {
    public BillingMainVM()
    {

    }

    public override void Initialise()
    {
      base.Initialise();

      this.BillingMenu.Add(UserInterfaces.Cargo.ForeignBills);
      this.BillingMenu.Add(UserInterfaces.Cargo.DomesticBills);
      this.BillingMenu.Add(UserInterfaces.Cargo.RoroBills);
      //this.BillingMenu.Add(UserInterfaces.Cargo.WalkInBills);
      this.BillingMenu.Add(UserInterfaces.Cargo.Payments);

      this.SelectedMenu = this.BillingMenu[0];
    }

    private List<CoreConstants.UserInterface> _BillingMenu = new List<CoreConstants.UserInterface>();
    public List<CoreConstants.UserInterface> BillingMenu 
    {
      get { return _BillingMenu; }
      set { _BillingMenu = value; }
    }

    private CoreConstants.UserInterface _SelectedMenu;
    public CoreConstants.UserInterface SelectedMenu
    {
      get { return _SelectedMenu; }
      set
      {
        _SelectedMenu = value;

        var userinterface = UserInterfaces.FindById(value.Id);
        if (userinterface != null)
        {
          NavigationManager.Show(userinterface);
        }
      }
    }
  }
}
