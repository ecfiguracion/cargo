using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitz.Core.Constants;

namespace Bitz.Core.ViewModel
{
  public interface IViewModelBase
  {
    bool IsReadOnly { get; set; }
    bool IsBusy { get; set; }
    CoreConstants.UserInterface UserInterface { get; set; }

    void Initialise();
  }
}
