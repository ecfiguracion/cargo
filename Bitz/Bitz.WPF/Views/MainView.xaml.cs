using Bitz.Core.Application;
using Bitz.Core.Shell;
using Bitz.Core.ViewModel;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bitz.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainView : ModernWindow 
  {
    public MainView()
    {
      InitializeComponent();
      AppearanceManager.Current.AccentColor = Colors.Orange;

      AppCache.MainFrame = this.xMainFrame;

      var vm = new MainVM();

      this.DataContext = vm;      
      vm.SelectedLink = this.Menu.LinkGroups[0].Links[0];
    }
  }
}
