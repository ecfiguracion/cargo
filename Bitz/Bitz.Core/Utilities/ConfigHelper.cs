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

    public static string GetReportsPath()
    {
      return ConfigurationManager.AppSettings["ReportsPath"];
    }

    public static int GetPageSize()
    {
      return int.Parse(ConfigurationManager.AppSettings["PageSize"]);
    }


    #endregion
  }
}
