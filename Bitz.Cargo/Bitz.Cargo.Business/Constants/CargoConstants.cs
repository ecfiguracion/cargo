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

    #region BillStatus

    public class BillStatus
    {
      private static List<CoreConstants.IdValue> _BillStatus = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Draft { get { return _BillStatus[0]; } }
      public static CoreConstants.IdValue PartiallyPaid { get { return _BillStatus[1]; } }
      public static CoreConstants.IdValue FullyPaid { get { return _BillStatus[2]; } }

      static BillStatus()
      {
        _BillStatus.Add(new CoreConstants.IdValue(0, "Draft"));
        _BillStatus.Add(new CoreConstants.IdValue(1, "Partially Paid"));
        _BillStatus.Add(new CoreConstants.IdValue(2, "Fully Paid"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _BillStatus; }
      }
    }

    #endregion

    #region WeightRates

    public class WeightRates
    {
      private static List<CoreConstants.IdValue> _WeightRates = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue MetricTons { get { return _WeightRates[0]; } }
      public static CoreConstants.IdValue RevenueTons { get { return _WeightRates[1]; } }

      static WeightRates()
      {
        _WeightRates.Add(new CoreConstants.IdValue(0, "MT"));
        _WeightRates.Add(new CoreConstants.IdValue(1, "RT"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _WeightRates; }
      }
    }

    #endregion

  }
}
