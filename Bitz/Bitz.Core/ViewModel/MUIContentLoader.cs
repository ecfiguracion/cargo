using FirstFloor.ModernUI.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitz.Core.Constants;
using System.Windows;
using Bitz.Core.Application;
using FirstFloor.ModernUI.Windows.Controls;
using System.Reflection;

namespace Bitz.Core.ViewModel
{
  public class MUIContentLoader : IContentLoader
  {
    private CoreConstants.UserInterface UserInterface { get; set; }
    private object[] Parameters { get; set; }

    public MUIContentLoader(CoreConstants.UserInterface userInterface,object[] parameter)
    {
      this.UserInterface = userInterface;
      this.Parameters = parameter;
    }
 
    public async Task<object> LoadContentAsync(Uri uri, System.Threading.CancellationToken cancellationToken) {

      var type = Type.GetType(this.UserInterface.Assembly);

      //Create instance of view
      var page = type.Assembly.CreateInstance(UserInterface.Page);

      //Assign the view model
      FrameworkElement pageelement = page as FrameworkElement;

      //Track down page with content page
      var contentElement = pageelement.FindName(CoreConstants.Region.ContentRegion.ToString());
      if (contentElement != null)
      {
        AppCache.ContentFrame = contentElement as ModernFrame;
        AppCache.ContentFrame.KeepContentAlive = false;
      }

      //Mary the ViewModel to View
      var viewmodel = pageelement.Resources["ViewModel"];
      if (viewmodel != null)
      {
        pageelement.DataContext = viewmodel;
      }

      Type viewmodelType = viewmodel.GetType();

      // Set the UserInterface property
      PropertyInfo userinterfaceProperty = viewmodelType.GetProperty("UserInterface");
      if (userinterfaceProperty != null)
      {
        userinterfaceProperty.SetValue(viewmodel, this.UserInterface, null);
      }

      //Pass parameter to VM's Initialise
      viewmodelType.InvokeMember("Initialise", System.Reflection.BindingFlags.InvokeMethod, null,
                        viewmodel,this.Parameters);

      return page;
    }
  }
}
