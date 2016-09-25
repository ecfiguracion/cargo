using Bitz.Core.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Utilities
{
  public class ConfigHelper
  {
    #region System Configurations

    public static string GetDatabase()
    {
      return ConfigurationManager.ConnectionStrings["Bitz"].ConnectionString;
    }

    public static string GetAppSettings(CoreConstants.IdValue configuration)
    {
      return ConfigurationManager.AppSettings[configuration.Value];
    }


    #endregion
  }
}
