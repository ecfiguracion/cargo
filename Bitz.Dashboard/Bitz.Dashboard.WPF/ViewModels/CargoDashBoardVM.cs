using Bitz.Core.Application;
using Bitz.Core.Shell;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Dashboard.ViewModels
{
  public class CargoDashBoardVM : GridViewModelBase<object>
  {
    public CargoDashBoardVM()
    {

    }

    public override void Initialise()
    {
      base.Initialise();
    }

    public string Info
    {
      get
      {
        return "Hello demo!";
      }
    }
  }
}
