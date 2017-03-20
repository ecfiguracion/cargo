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

namespace Bitz.Reports.Views
{
  /// <summary>
  /// Interaction logic for RPT0008View.xaml
  /// </summary>
  public partial class RPT0008View : ModernDialog
  {
    public RPT0008View()
    {
      InitializeComponent();

      // define the dialog buttons
      this.Buttons = new Button[] { this.OkButton, this.CancelButton };
    }
  }
}
