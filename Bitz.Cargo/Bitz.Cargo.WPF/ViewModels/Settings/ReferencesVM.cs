using Bitz.Core.Application;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitz.Core.Constants;
using Bitz.Core.Shell;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class ReferencesVM : GridViewModelBase<object>
  {
    public ReferencesVM()
    {

    }

    public override void Initialise()
    {
      base.Initialise();

      this.BillingMenu.Add(UserInterfaces.Cargo.Consignees);
      this.BillingMenu.Add(UserInterfaces.Cargo.Vessels);
      this.BillingMenu.Add(UserInterfaces.Cargo.CargoItems);
      this.BillingMenu.Add(UserInterfaces.Cargo.UnitOfMeasures);

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
