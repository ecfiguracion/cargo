using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bitz.Core.Application
{
  public static class AppCache
  {
    public static ModernFrame MainFrame { get; set; }

    public static ModernFrame ContentFrame { get; set; }

    public static ModernWindow MainWindow { get; set; }

  }
}
