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
  #region BillPaymentInfo

  [Serializable]
  [TableInfo(TableName = "bill", KeyColumn = "bill.bill")]
  public class BillPaymentInfo : ReadOnlyBase<BillPaymentInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Consignee

    public static readonly PropertyInfo<string> _Consignee = RegisterProperty<string>(c => c.Consignee);
    public string Consignee
    {
      get { return GetProperty(_Consignee); }
    }

    #endregion

    #region BillNo

    public static readonly PropertyInfo<string> _BillNo = RegisterProperty<string>(c => c.BillNo);
    public string BillNo
    {
      get { return GetProperty(_BillNo); }
    }

    #endregion

    #region BillDate

    public static readonly PropertyInfo<SmartDate> _BillDate = RegisterProperty<SmartDate>(c => c.BillDate);
    public SmartDate BillDate
    {
      get { return GetProperty(_BillDate); }
    }

    #endregion

    #region TotalBill

    public static readonly PropertyInfo<decimal> _TotalBill = RegisterProperty<decimal>(c => c.TotalBill);
    public decimal TotalBill
    {
      get { return GetProperty(_TotalBill); }
    }

    #endregion

    #region AmountPaid

    public static readonly PropertyInfo<decimal> _AmountPaid = RegisterProperty<decimal>(c => c.AmountPaid);
    public decimal AmountPaid
    {
      get { return GetProperty(_AmountPaid); }
    }

    #endregion

    #region AmountDue

    public static readonly PropertyInfo<decimal> _AmountDue = RegisterProperty<decimal>(c => c.AmountDue);
    public decimal AmountDue
    {
      get 
      {
        return this.TotalBill - this.AmountPaid;
      }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static BillPaymentInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillPaymentInfo>(dr,string.Empty);
    }

    public static BillPaymentInfo Get(SafeDataReader dr, string columnprefix)
    {
      return Csla.DataPortal.FetchChild<BillPaymentInfo>(dr, columnprefix);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr,string columnprefix)
    {
      LoadProperty(_Id, dr.GetInt32(columnprefix + "bill"));

      LoadProperty(_BillNo, dr.GetString(columnprefix + "billno"));
      LoadProperty(_BillDate, dr.GetSmartDate(columnprefix + "billdate"));

      if (SQLHelper.HasColumn(dr, columnprefix+"consignee"))
        LoadProperty(_Consignee, dr.GetString(columnprefix + "consignee"));
      if (SQLHelper.HasColumn(dr,columnprefix+"totalbill"))
        LoadProperty(_TotalBill, dr.GetDecimal(columnprefix + "totalbill"));
      if (SQLHelper.HasColumn(dr,columnprefix+"totalamountpaid"))
        LoadProperty(_AmountPaid, dr.GetDecimal(columnprefix + "totalamountpaid"));

    }

    #endregion

    #endregion

    #endregion
  }

  #endregion

  #region BillPaymentInfos

  [Serializable]
  public class BillPaymentInfos : ReadOnlyListBase<BillPaymentInfos, BillPaymentInfo>
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

      #region BillTypes

      public static readonly PropertyInfo<MobileList<int>> _BillTypes = RegisterProperty<MobileList<int>>(c => c.BillTypes);
      public MobileList<int> BillTypes
      {
        get { return ReadProperty(_BillTypes); }
        set { LoadProperty(_BillTypes, value); }
      }

      #endregion

      #region Consignee

      public static readonly PropertyInfo<int?> _Consignee = RegisterProperty<int?>(c => c.Consignee);
      public int? Consignee
      {
        get { return ReadProperty(_Consignee); }
        set { LoadProperty(_Consignee, value); }
      }

      #endregion
      
      #region Status

      public static readonly PropertyInfo<int?> _Status = RegisterProperty<int?>(c => c.Status);
      public int? Status
      {
        get { return ReadProperty(_Status); }
        set { LoadProperty(_Status, value); }
      }

      #endregion

      #region Statuses

      public static readonly PropertyInfo<MobileList<int>> _Statuses = RegisterProperty<MobileList<int>>(c => c.Statuses);
      public MobileList<int> Statuses
      {
        get { return ReadProperty(_Statuses); }
        set { LoadProperty(_Statuses, value); }
      }

      #endregion

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<BillPaymentInfos>> completed)
    {
      DataPortal<BillPaymentInfos> dp = new DataPortal<BillPaymentInfos>();
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
          cmd.CommandText = @"SELECT b.bill,c.name as consignee,b.billno,b.billdate,b.totalbill,sum(pd.amountpaid) totalamountpaid
                              FROM bill b
                              LEFT JOIN contact c ON b.consignee = c.contact
                              LEFT JOIN paymentdetail pd ON b.bill = pd.bill
                              WHERE (b.status = @status OR @status IS NULL)
                              AND (b.consignee = @consignee OR @consignee IS NULL)";

          if (!string.IsNullOrEmpty(criteria.SearchText))
          {
            cmd.CommandText += @" AND (b.billno LIKE @SearchText 
                                  OR c.name LIKE @SearchText)";

            cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");
          }

          if (criteria.Status != null)
            cmd.Parameters.AddWithValue("@status", criteria.Status);
          else
            cmd.Parameters.AddWithValue("@status", DBNull.Value);

          if (criteria.Consignee != null)
            cmd.Parameters.AddWithValue("@consignee", criteria.Consignee);
          else
            cmd.Parameters.AddWithValue("@consignee", DBNull.Value);

          if (criteria.BillTypes != null && criteria.BillTypes.Count > 0)
          {
            cmd.CommandText += " AND b.billtype IN (" + string.Join(",", criteria.BillTypes.Select(x => x)) + ")";
          }

          if (criteria.Statuses != null && criteria.Statuses.Count > 0)
          {
            cmd.CommandText += " AND b.status IN (" + string.Join(",", criteria.Statuses.Select(x => x)) + ")";
          }

          cmd.CommandText += " GROUP BY b.bill,c.name,b.billno,b.billdate,b.totalbill";

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "b.billdate DESC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(BillPaymentInfo.Get(dr));
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
