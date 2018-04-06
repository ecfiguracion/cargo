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
      public static CoreConstants.IdValue WalkIn { get { return _BillingType[3]; } }
      public static CoreConstants.IdValue Mooring { get { return _BillingType[4]; } }
      public static CoreConstants.IdValue Porterage { get { return _BillingType[5]; } }

      static BillingType()
      {
        _BillingType.Add(new CoreConstants.IdValue(1, "FOREIGN"));
        _BillingType.Add(new CoreConstants.IdValue(2, "DOMESTIC"));
        _BillingType.Add(new CoreConstants.IdValue(3, "RORO"));
        _BillingType.Add(new CoreConstants.IdValue(4, "WALKIN"));
        _BillingType.Add(new CoreConstants.IdValue(5, "MOORING/UNMOORING"));
        _BillingType.Add(new CoreConstants.IdValue(6, "PORTERAGE"));
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

      public static CoreConstants.IdValue All { get { return _BillStatus[0]; } }
      public static CoreConstants.IdValue Draft { get { return _BillStatus[1]; } }
      public static CoreConstants.IdValue PartiallyPaid { get { return _BillStatus[2]; } }
      public static CoreConstants.IdValue FullyPaid { get { return _BillStatus[3]; } }
      public static CoreConstants.IdValue Cancelled { get { return _BillStatus[4]; } }

      static BillStatus()
      {
        _BillStatus.Add(new CoreConstants.IdValue(0, "ALL"));
        _BillStatus.Add(new CoreConstants.IdValue(1, "Draft"));
        _BillStatus.Add(new CoreConstants.IdValue(2, "Partially Paid"));
        _BillStatus.Add(new CoreConstants.IdValue(3, "Fully Paid"));
        _BillStatus.Add(new CoreConstants.IdValue(4, "Cancelled"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _BillStatus; }
      }
    }

    #endregion

    #region StandardStatus

    public class StandardStatus
    {
      private static List<CoreConstants.IdValue> _StandardStatus = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue All { get { return _StandardStatus[0]; } }
      public static CoreConstants.IdValue Draft { get { return _StandardStatus[1]; } }
      public static CoreConstants.IdValue Cancelled { get { return _StandardStatus[2]; } }
      public static CoreConstants.IdValue Completed { get { return _StandardStatus[3]; } }

      static StandardStatus()
      {
        _StandardStatus.Add(new CoreConstants.IdValue(0, "ALL"));
        _StandardStatus.Add(new CoreConstants.IdValue(1, "Draft"));
        _StandardStatus.Add(new CoreConstants.IdValue(2, "Cancelled"));
        _StandardStatus.Add(new CoreConstants.IdValue(3, "Completed"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _StandardStatus; }
      }
    }

    #endregion

    #region PaymentStatus

    public class PaymentStatus
    {
      private static List<CoreConstants.IdValue> _PaymentStatus = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Draft { get { return _PaymentStatus[0]; } }
      public static CoreConstants.IdValue Approved { get { return _PaymentStatus[1]; } }

      static PaymentStatus()
      {
        _PaymentStatus.Add(new CoreConstants.IdValue(0, "Draft"));
        _PaymentStatus.Add(new CoreConstants.IdValue(1, "Approved"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _PaymentStatus; }
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

    #region MooringTypes

    public class MooringType
    {
      private static List<CoreConstants.IdValue> _MooringType = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue All { get { return _MooringType[0]; } }
      public static CoreConstants.IdValue Domestic { get { return _MooringType[1]; } }
      public static CoreConstants.IdValue Foreign { get { return _MooringType[2]; } }

      static MooringType()
      {
        _MooringType.Add(new CoreConstants.IdValue(0, "ALL"));
        _MooringType.Add(new CoreConstants.IdValue(1, "Domestic"));
        _MooringType.Add(new CoreConstants.IdValue(2, "Foreign"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _MooringType; }
      }
    }

    #endregion

    #region PaymentTypes

    public class PaymentTypes
    {
      private static List<CoreConstants.IdValue> _PaymentTypes = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Cash { get { return _PaymentTypes[0]; } }
      public static CoreConstants.IdValue Check { get { return _PaymentTypes[1]; } }
      public static CoreConstants.IdValue Online { get { return _PaymentTypes[2]; } }

      static PaymentTypes()
      {
        _PaymentTypes.Add(new CoreConstants.IdValue(0, "Cash"));
        _PaymentTypes.Add(new CoreConstants.IdValue(1, "Check"));
        _PaymentTypes.Add(new CoreConstants.IdValue(2, "Online"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _PaymentTypes; }
      }
    }

    #endregion

    #region SignatoryTypes

    public class SignatoryTypes
    {
      private static List<CoreConstants.IdValue> _SignatoryTypes = new List<CoreConstants.IdValue>();

      public static CoreConstants.IdValue Cash { get { return _SignatoryTypes[0]; } }
      public static CoreConstants.IdValue Check { get { return _SignatoryTypes[1]; } }

      static SignatoryTypes()
      {
        _SignatoryTypes.Add(new CoreConstants.IdValue(1, "Disbursement - Prepared By"));
        _SignatoryTypes.Add(new CoreConstants.IdValue(2, "Disbursement - Approved By"));
      }

      public static List<CoreConstants.IdValue> Items
      {
        get { return _SignatoryTypes; }
      }
    }

    #endregion
  }
}
