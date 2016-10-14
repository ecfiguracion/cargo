using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.Data;
using Bitz.Core.Utilities;
using Csla;
using Csla.Core;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Billing.Infos
{
  #region PaymentInfo

  [Serializable]
  [TableInfo(TableName = "bill", KeyColumn = "bill.bill")]
  public class PaymentInfo : ReadOnlyBase<PaymentInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region PaymentNo

    public static readonly PropertyInfo<string> _PaymentNo = RegisterProperty<string>(c => c.PaymentNo);
    public string PaymentNo
    {
      get { return GetProperty(_PaymentNo); }
    }

    #endregion

    #region PaymentDate

    public static readonly PropertyInfo<SmartDate> _PaymentDate = RegisterProperty<SmartDate>(c => c.PaymentDate);
    public SmartDate PaymentDate
    {
      get { return GetProperty(_PaymentDate); }
    }

    #endregion

    #region Consignee

    public static readonly PropertyInfo<string> _Consignee = RegisterProperty<string>(c => c.Consignee);
    public string Consignee
    {
      get { return GetProperty(_Consignee); }
    }

    #endregion

    #region AmountPaid

    public static readonly PropertyInfo<decimal> _AmountPaid = RegisterProperty<decimal>(c => c.AmountPaid);
    public decimal AmountPaid
    {
      get { return GetProperty(_AmountPaid); }
    }

    #endregion

    #region Status

    public static readonly PropertyInfo<int> _Status = RegisterProperty<int>(c => c.Status);
    public CoreConstants.IdValue Status
    {
      get
      {
        return CargoConstants.PaymentStatus.Items.SingleOrDefault(x => x.Id == GetProperty(_Status));
      }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static PaymentInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<PaymentInfo>(dr);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("payment"));
      LoadProperty(_PaymentNo, dr.GetString("paymentno"));
      LoadProperty(_PaymentDate, dr.GetSmartDate("paymentdate"));
      LoadProperty(_Consignee, dr.GetString("consignee"));
      LoadProperty(_AmountPaid, dr.GetDecimal("amountpaid"));
      LoadProperty(_Status, dr.GetInt32("status"));
    }

    #endregion

    #endregion

    #endregion
  }

  #endregion

  #region PaymentInfos

  [Serializable]
  public class PaymentInfos : ReadOnlyListBase<PaymentInfos, PaymentInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : PageCriteriaBase<Criteria>
    {
      #region SearchText
      private static PropertyInfo<string> _SearchText = RegisterProperty<string>(c => c.SearchText);

      public string SearchText
      {
        get { return ReadProperty(_SearchText); }
        set { LoadProperty(_SearchText, value); }
      }
      #endregion //SearchText

      #region Status

      public static readonly PropertyInfo<int?> _Status = RegisterProperty<int?>(c => c.Status);
      public int? Status
      {
        get { return ReadProperty(_Status); }
        set { LoadProperty(_Status, value); }
      }

      #endregion

      #region StartDate

      public static readonly PropertyInfo<SmartDate> _StartDate = RegisterProperty<SmartDate>(c => c.StartDate);
      public SmartDate StartDate
      {
        get { return ReadProperty(_StartDate); }
        set { LoadProperty(_StartDate, value); }
      }

      #endregion

      #region EndDate

      public static readonly PropertyInfo<SmartDate> _EndDate = RegisterProperty<SmartDate>(c => c.EndDate);
      public SmartDate EndDate
      {
        get { return ReadProperty(_EndDate); }
        set { LoadProperty(_EndDate, value); }
      }

      #endregion
    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<PaymentInfos>> completed)
    {
      DataPortal<PaymentInfos> dp = new DataPortal<PaymentInfos>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(criteria);
    }

    #endregion

    #region Data Access

    protected void DataPortal_Fetch(Criteria criteria)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT p.payment,p.paymentno,p.paymentdate,c.name as consignee, p.status,sum(pd.amountpaid) amountpaid
                              FROM payment p
                              INNER JOIN contact c ON p.consignee = c.contact
                              LEFT JOIN paymentdetail pd ON p.payment = pd.payment
                              WHERE (p.status = @status OR @status IS NULL)";
          if (!string.IsNullOrEmpty(criteria.SearchText))
          {
            cmd.CommandText += @" AND (b.paymentno LIKE @SearchText 
                                  OR c.name LIKE @SearchText)";
            cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");
          }

          if (criteria.Status != null)
            cmd.Parameters.AddWithValue("@status", criteria.Status);
          else
            cmd.Parameters.AddWithValue("@status", DBNull.Value);

          if (criteria.StartDate != null && criteria.EndDate != null)
          {
            cmd.CommandText += @" AND (b.billdate >= @fromDate AND b.billingdate < @toDate)";
            cmd.Parameters.AddWithValue("@fromDate", criteria.StartDate.Date);
            cmd.Parameters.AddWithValue("@toDate", criteria.EndDate.Date.AddDays(1));
          }

          cmd.CommandText += "GROUP BY p.payment,p.paymentno,p.paymentdate,c.name, p.status";

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "p.paymentdate DESC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(PaymentInfo.Get(dr));
            }
            IsReadOnly = true;
          }
        }
      }
    }

    #endregion

  }

  #endregion
}
