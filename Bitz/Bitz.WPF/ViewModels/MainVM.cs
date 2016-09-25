using Bitz.Core.Application;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitz.Core;
using Bitz.Core.Constants;

namespace Bitz
{
  public class MainVM
  {
    public MainVM()
    {
    }

    #region Properties
    
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

    public string Company
    {
      get
      {
        return ConfigHelper.GetAppSettings(BitzConstants.Configurations.Company);
      }
    }

    public string AppName
    {
      get
      {
        return ConfigHelper.GetAppSettings(BitzConstants.Configurations.AppName);
      }
    }

    public string LogoPath
    {
      get
      {
        return ConfigHelper.GetAppSettings(BitzConstants.Configurations.LogoPath);
      }
    }

    #endregion

  }
}
