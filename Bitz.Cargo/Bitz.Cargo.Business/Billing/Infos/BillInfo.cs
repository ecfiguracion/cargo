using Bitz.Cargo.Business.Constants;
using Bitz.Core.Constants;
using Bitz.Core.Data;
using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Billing.Infos
{
  #region BillInfo

  [Serializable]
  [TableInfo(TableName = "bill", KeyColumn = "bill.bill")]
  public class BillInfo : ReadOnlyBase<BillInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
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

    #region Consignee

    public static readonly PropertyInfo<string> _Consignee = RegisterProperty<string>(c => c.Consignee);
    public string Consignee
    {
      get { return GetProperty(_Consignee); }
    }

    #endregion

    #region Vessel

    public static readonly PropertyInfo<string> _Vessel = RegisterProperty<string>(c => c.Vessel);
    public string Vessel
    {
      get { return GetProperty(_Vessel); }
    }

    #endregion

    #region VoyageNo

    public static readonly PropertyInfo<string> _VoyageNo = RegisterProperty<string>(c => c.VoyageNo);
    public string VoyageNo
    {
      get { return GetProperty(_VoyageNo); }
    }

    #endregion

    #region ORNumber

    public static readonly PropertyInfo<string> _ORNumber = RegisterProperty<string>(c => c.ORNumber);
    public string ORNumber
    {
      get { return GetProperty(_ORNumber); }
    }

    #endregion

    #region Status

    public static readonly PropertyInfo<int> _Status = RegisterProperty<int>(c => c.Status);
    public CoreConstants.IdValue Status
    {
      get
      {
        return CargoConstants.BillStatus.Items.SingleOrDefault(x => x.Id == GetProperty(_Status));
      }
    }

    #endregion

    #region MooringType

    public static readonly PropertyInfo<int> _MooringType = RegisterProperty<int>(c => c.MooringType);
    public CoreConstants.IdValue MooringType
    {
      get
      {
        return CargoConstants.MooringType.Items.SingleOrDefault(x => x.Id == GetProperty(_MooringType));
      }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static BillInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<BillInfo>> completed)
    {
      DataPortal<BillInfo> dp = new DataPortal<BillInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("bill"));
      LoadProperty(_BillNo, dr.GetString("billno"));
      LoadProperty(_BillDate, dr.GetSmartDate("billdate"));
      LoadProperty(_Consignee, dr.GetString("consignee"));
      LoadProperty(_Vessel, dr.GetString("vessel"));
      LoadProperty(_VoyageNo, dr.GetString("voyageno"));
      LoadProperty(_ORNumber, dr.GetString("ornumber"));
      LoadProperty(_Status, dr.GetInt32("status"));
      LoadProperty(_MooringType, dr.GetInt32("mooringtype"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM bill WHERE bill = @id";
          cm.Parameters.AddWithValue("@id", id.Value);
          cm.ExecuteNonQuery();
        }
      }
    }

    #endregion

    #endregion

    #endregion
  }

  #endregion

  #region BillInfos

  [Serializable]
  public class BillInfos : ReadOnlyListBase<BillInfos, BillInfo>
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

      #region MooringType

      public static readonly PropertyInfo<int?> _MooringType = RegisterProperty<int?>(c => c.MooringType);
      public int? MooringType
      {
        get { return ReadProperty(_MooringType); }
        set { LoadProperty(_MooringType, value); }
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

      #region BillType
      private static PropertyInfo<int?> _BillType = RegisterProperty<int?>(c => c.BillType);

      public int? BillType
      {
        get { return ReadProperty(_BillType); }
        set { LoadProperty(_BillType, value); }
      }
      #endregion 

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<BillInfos>> completed)
    {
      DataPortal<BillInfos> dp = new DataPortal<BillInfos>();
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
          cmd.CommandText = @"SELECT b.bill,b.billno,b.billdate,c.name as consignee,v.name as vessel,b.voyageno,b.ornumber,b.status,b.mooringtype
                              FROM bill b
                              LEFT JOIN contact c ON b.consignee = c.contact
                              LEFT JOIN contact v ON b.vessel = v.contact
                              WHERE (b.billtype = @billtype OR @billtype IS NULL)
                              AND (b.status = @status OR @status IS NULL)
                              AND (b.mooringtype = @mooringtype OR @mooringtype IS NULL)";
          if (!string.IsNullOrEmpty(criteria.SearchText))
          {
            cmd.CommandText += @" AND (b.billno LIKE @SearchText 
                                  OR c.name LIKE @SearchText 
                                  OR v.name LIKE @SearchText 
                                  OR b.voyageno LIKE @SearchText 
                                  OR b.ornumber LIKE @SearchText)";

            cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");
          }

          if (criteria.BillType != null)
            cmd.Parameters.AddWithValue("@billtype", criteria.BillType);
          else
            cmd.Parameters.AddWithValue("@billtype", DBNull.Value);

          if (criteria.Status != null && criteria.Status > 0)
            cmd.Parameters.AddWithValue("@status", criteria.Status);
          else
            cmd.Parameters.AddWithValue("@status", DBNull.Value);

          if (criteria.StartDate != null && criteria.EndDate != null)
          {
            cmd.CommandText += @" AND (b.billdate >= @fromDate AND b.billingdate < @toDate)";
            cmd.Parameters.AddWithValue("@fromDate", criteria.StartDate.Date);
            cmd.Parameters.AddWithValue("@toDate", criteria.EndDate.Date.AddDays(1));
          }

          if (criteria.MooringType != null && criteria.MooringType > 0)
            cmd.Parameters.AddWithValue("@mooringtype", criteria.MooringType);
          else
            cmd.Parameters.AddWithValue("@mooringtype", DBNull.Value);

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
              this.Add(BillInfo.Get(dr));
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
