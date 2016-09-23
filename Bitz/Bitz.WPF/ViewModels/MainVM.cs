using Bitz.Core.Application;
using Bitz.Core.Shell;
using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz
{
  public class MainVM
  {
    public MainVM()
    {
    }

    private Link _SelectedLink;
    public Link SelectedLink
    {
      get { return _SelectedLink; }
      set
      {
        _SelectedLink = value;

        var userinterface = UserInterfaces.FindByName(value.DisplayName);
        if (userinterface != null)
        {
          NavigationManager.Show(userinterface);
        }
      }
    }

  }
}
