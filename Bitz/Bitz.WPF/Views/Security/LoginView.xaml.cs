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

namespace Bitz.Views.Security
{
  /// <summary>
  /// Interaction logic for LoginView.xaml
  /// </summary>
  public partial class LoginView : ModernDialog
  {
    public LoginView()
    {
      InitializeComponent();

      // define the dialog buttons
      this.Buttons = null;
    }

    private void xLoginButton_Click(object sender, RoutedEventArgs e)
    {
      MainView main = new MainView();
      main.Show();
      this.Close();
    }

    private void xCancelButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

  }
}
