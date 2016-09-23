using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Bitz.Helper
{
  public static class NavigationHelper
  {
    public static NavigationService _NavigationService;
    public static NavigationService NavigationService
    {
      get { return _NavigationService; }
      set { _NavigationService = value; }
    }
  }
}
