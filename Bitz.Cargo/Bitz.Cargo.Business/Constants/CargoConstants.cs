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
  }
}
