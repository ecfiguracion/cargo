using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Bitz.Core.Shell
{
  public static class NavigationManager
  {
    #region Show
    public static void Show(CoreConstants.UserInterface userinterface)
    {
      Show(userinterface, null);
    }

    public static void Show(CoreConstants.UserInterface userinterface, object[] parameter)
    {
      if (userinterface.PageType == CoreConstants.PageType.Page)
      {
        ShowPage(userinterface, parameter);
      }
      else if (userinterface.PageType == CoreConstants.PageType.ContentPage) 
      {
        ShowChildPage(userinterface, parameter);
      }
      else if (userinterface.PageType == CoreConstants.PageType.Dialog)
      {
        ShowDialog(userinterface);
      }
    }
    #endregion

    #region ShowPage
    private static void ShowPage(CoreConstants.UserInterface userinterface, object[] parameter)
    {
      if (AppCache.MainFrame != null)
      {
        AppCache.MainFrame.ContentLoader = new MUIContentLoader(userinterface, parameter);
        AppCache.MainFrame.Source = userinterface.Uri;
      }
    }
    #endregion

    #region ShowChildPage
    private static void ShowChildPage(CoreConstants.UserInterface userinterface, object[] parameter)
    {
      if (AppCache.ContentFrame != null)
      {       
        AppCache.ContentFrame.ContentLoader = new MUIContentLoader(userinterface, parameter);
        AppCache.ContentFrame.Source = userinterface.Uri;
      }
    }
    #endregion

    #region ShowDialog

    private static void ShowDialog(CoreConstants.UserInterface userinterface)
    {

    }

    #endregion

  }
}
