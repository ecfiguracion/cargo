using Bitz.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Constants
{
  public class CargoConstants
  {
    #region BillingType

    public class BillingType
    {
      private static List<CoreConstants.IdValue> _BillingType = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Foreign { get { return _BillingType[0]; } }
      public static CoreConstants.IdValue Domestic { get { return _BillingType[1]; } }
      public static CoreConstants.IdValue Roro { get { return _BillingType[2]; } }

      static BillingType()
      {
        _BillingType.Add(new CoreConstants.IdValue(1, "FOREIGN"));
        _BillingType.Add(new CoreConstants.IdValue(2, "DOMESTIC"));
        _BillingType.Add(new CoreConstants.IdValue(3, "RORO"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _BillingType; }
      }
    }

    #endregion

    #region CargoHandlingCharges

    public class CargoHandlingCharges
    {
      private static List<CoreConstants.IdValue> _CargoHandlingCharges = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Arrastre { get { return _CargoHandlingCharges[0]; } }
      public static CoreConstants.IdValue Stevedoring { get { return _CargoHandlingCharges[1]; } }
      public static CoreConstants.IdValue RollOnRollOff { get { return _CargoHandlingCharges[2]; } }

      static CargoHandlingCharges()
      {
        _CargoHandlingCharges.Add(new CoreConstants.IdValue(0, "Arrastre"));
        _CargoHandlingCharges.Add(new CoreConstants.IdValue(1, "Stevedoring"));
        _CargoHandlingCharges.Add(new CoreConstants.IdValue(2, "RollOn/RollOff"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _CargoHandlingCharges; }
      }
    }

    #endregion

    #region CargoHandlingChargeTypes

    public class CargoHandlingChargeTypes
    {
      private static List<CoreConstants.IdValue> _CargoHandlingChargeTypes = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Add { get { return _CargoHandlingChargeTypes[0]; } }
      public static CoreConstants.IdValue Less { get { return _CargoHandlingChargeTypes[1]; } }

      static CargoHandlingChargeTypes()
      {
        _CargoHandlingChargeTypes.Add(new CoreConstants.IdValue(0, "Add"));
        _CargoHandlingChargeTypes.Add(new CoreConstants.IdValue(1, "Less"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _CargoHandlingChargeTypes; }
      }
    }

    #endregion

  }
}
