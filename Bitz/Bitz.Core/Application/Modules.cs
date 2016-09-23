using Bitz.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Application
{
  public static class Modules
  {
    private static List<CoreConstants.Module> _modules = new List<CoreConstants.Module>();

    public static CoreConstants.Module Bitz { get { return _modules[0]; } }
    public static CoreConstants.Module DashBoard { get { return _modules[1]; } }
    public static CoreConstants.Module Reports { get { return _modules[2]; } }
    public static CoreConstants.Module Cargo { get { return _modules[3]; } }
    public static CoreConstants.Module Payroll { get { return _modules[4]; } }

    static Modules()
    {
      _modules.Add(new CoreConstants.Module(100, "Bitz", "Bitz"));
      _modules.Add(new CoreConstants.Module(101, "DashBoard", "Bitz.Dashboard"));
      _modules.Add(new CoreConstants.Module(102, "Reports", "Bitz.Reports"));
      _modules.Add(new CoreConstants.Module(200, "Cargo", "Bitz.Cargo"));
      _modules.Add(new CoreConstants.Module(300, "Payroll", "Bitz.Payroll"));
    }
  }
}
