using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Constants
{
  public class BitzConstants
  {
    #region Configurations

    public class Configurations
    {
      private static List<CoreConstants.IdValue> _Configurations = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue ReportsPath { get { return _Configurations[0]; } }
      public static CoreConstants.IdValue PageSize { get { return _Configurations[1]; } }
      public static CoreConstants.IdValue Company { get { return _Configurations[2]; } }
      public static CoreConstants.IdValue AppName { get { return _Configurations[3]; } }
      public static CoreConstants.IdValue LogoPath { get { return _Configurations[4]; } }

      static Configurations()
      {
        _Configurations.Add(new CoreConstants.IdValue(1, "ReportsPath"));
        _Configurations.Add(new CoreConstants.IdValue(2, "PageSize"));
        _Configurations.Add(new CoreConstants.IdValue(3, "Company"));
        _Configurations.Add(new CoreConstants.IdValue(4, "AppName"));
        _Configurations.Add(new CoreConstants.IdValue(5, "LogoPath"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _Configurations; }
      }
    }

    #endregion

    #region ContactTypes

    public class ContactTypes
    {
      private static List<CoreConstants.IdValue> _ContactTypes = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Employee { get { return _ContactTypes[0]; } }
      public static CoreConstants.IdValue Consignee { get { return _ContactTypes[1]; } }
      public static CoreConstants.IdValue Vessel { get { return _ContactTypes[2]; } }

      static ContactTypes()
      {
        _ContactTypes.Add(new CoreConstants.IdValue(1, "Employee"));
        _ContactTypes.Add(new CoreConstants.IdValue(2, "Consignee"));
        _ContactTypes.Add(new CoreConstants.IdValue(3, "Vessel"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _ContactTypes; }
      }
    }

    #endregion

    #region TableCounter

    public class TableCounter
    {
      private static List<CoreConstants.IdValue> _TableCounter = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Item { get { return _TableCounter[0]; } }
      public static CoreConstants.IdValue Employee { get { return _TableCounter[1]; } }
      public static CoreConstants.IdValue Consignee { get { return _TableCounter[2]; } }
      public static CoreConstants.IdValue Vessel { get { return _TableCounter[3]; } }

      static TableCounter()
      {
        _TableCounter.Add(new CoreConstants.IdValue(1, "ITEM"));
        _TableCounter.Add(new CoreConstants.IdValue(2, "EMPLOYEE"));
        _TableCounter.Add(new CoreConstants.IdValue(3, "CONSIGNEE"));
        _TableCounter.Add(new CoreConstants.IdValue(4, "VESSEL"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _TableCounter; }
      }
    }

    #endregion

  }
}
