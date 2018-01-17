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
  public class FeesMainVM : GridViewModelBase<object>
  {
    public FeesMainVM()
    {

    }

    public override void Initialise()
    {
      base.Initialise();

      this.FeesMenu.Add(UserInterfaces.Cargo.RoroTerminalFees);
      this.SelectedMenu = this.FeesMenu[0];
    }

    private List<CoreConstants.UserInterface> _FeesMenu = new List<CoreConstants.UserInterface>();
    public List<CoreConstants.UserInterface> FeesMenu 
    {
      get { return _FeesMenu; }
      set { _FeesMenu = value; }
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
