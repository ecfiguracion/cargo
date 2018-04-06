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
  public class DisbursementMainVM : GridViewModelBase<object>
  {
    public DisbursementMainVM()
    {

    }

    public override void Initialise()
    {
      base.Initialise();

      this.DisbursementMenu.Add(UserInterfaces.Cargo.Disbursements);
      this.SelectedMenu = this.DisbursementMenu[0];
    }

    private List<CoreConstants.UserInterface> _DisbursementMenu = new List<CoreConstants.UserInterface>();
    public List<CoreConstants.UserInterface> DisbursementMenu 
    {
      get { return _DisbursementMenu; }
      set { _DisbursementMenu = value; }
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
