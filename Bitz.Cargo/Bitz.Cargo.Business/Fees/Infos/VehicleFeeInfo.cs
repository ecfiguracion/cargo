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

namespace Bitz.Cargo.Business.Fees.Infos
{
  #region VehicleFeeInfo

  [Serializable]
  [TableInfo(TableName = "vehiclefee", KeyColumn = "vehiclefee.vehiclefee")]
  public class VehicleFeeInfo : ReadOnlyBase<VehicleFeeInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region FeeNo

    public static readonly PropertyInfo<string> _FeeNo = RegisterProperty<string>(c => c.FeeNo);
    public string FeeNo
    {
      get { return GetProperty(_FeeNo); }
    }

    #endregion

    #region Date

    public static readonly PropertyInfo<DateTime> _Date = RegisterProperty<DateTime>(c => c.Date);
    public DateTime Date
    {
      get { return GetProperty(_Date); }
    }

    #endregion

    #region TotalVehicles

    public static readonly PropertyInfo<int> _TotalVehicles = RegisterProperty<int>(c => c.TotalVehicles);
    public int TotalVehicles
    {
      get { return GetProperty(_TotalVehicles); }
    }

    #endregion

    #region TotalServiceFees

    public static readonly PropertyInfo<decimal> _TotalServiceFees = RegisterProperty<decimal>(c => c.TotalServiceFees);
    public decimal TotalServiceFees
    {
      get { return GetProperty(_TotalServiceFees); }
    }

    #endregion

    #region Tax

    public static readonly PropertyInfo<decimal> _Tax = RegisterProperty<decimal>(c => c.Tax);
    public decimal Tax
    {
      get { return GetProperty(_Tax); }
    }

    #endregion

    #region GrandTotal

    public static readonly PropertyInfo<decimal> _GrandTotal = RegisterProperty<decimal>(c => c.GrandTotal);
    public decimal GrandTotal
    {
      get { return GetProperty(_GrandTotal); }
    }

    #endregion

    #region Status

    public static readonly PropertyInfo<int> _Status = RegisterProperty<int>(c => c.Status);
    public CoreConstants.IdValue Status
    {
      get
      {
        return CargoConstants.RoroStatus.Items.SingleOrDefault(x => x.Id == GetProperty(_Status));
      }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static VehicleFeeInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<VehicleFeeInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<VehicleFeeInfo>> completed)
    {
      DataPortal<VehicleFeeInfo> dp = new DataPortal<VehicleFeeInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("vehiclefee"));
      LoadProperty(_FeeNo, dr.GetString("feeno"));
      LoadProperty(_Date, dr.GetSmartDate("date"));
      LoadProperty(_TotalVehicles, dr.GetInt32("totalvehicles"));
      LoadProperty(_TotalServiceFees, dr.GetDecimal("totalservicefees"));
      LoadProperty(_Tax, dr.GetDecimal("tax"));
      LoadProperty(_GrandTotal, dr.GetDecimal("grandtotal"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM vehiclefee WHERE vehiclefee = @id";
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

  #region VehicleFeeInfos

  [Serializable]
  public class VehicleFeeInfos : ReadOnlyListBase<VehicleFeeInfos, VehicleFeeInfo>
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

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<VehicleFeeInfos>> completed)
    {
      DataPortal<VehicleFeeInfos> dp = new DataPortal<VehicleFeeInfos>();
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
          cmd.CommandText = @"select v.vehiclefee,feeno, date,COUNT(vi.vehicle) totalvehicles, SUM(vi.fee) as totalservicefees, 
                                v.ppatotal - SUM(vi.fee) as tax, v.ppatotal as grandtotal
                              from vehiclefee v
                              left join vehiclefeeitem vi on v.vehiclefee = vi.vehiclefee
                              where  (v.status = @status OR @status IS NULL)";

          if (!string.IsNullOrEmpty(criteria.SearchText))
          {
            cmd.CommandText += @" AND (vi.invoiceno LIKE @SearchText)";

            cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");
          }

          if (criteria.Status != null && criteria.Status > 0)
            cmd.Parameters.AddWithValue("@status", criteria.Status);
          else
            cmd.Parameters.AddWithValue("@status", DBNull.Value);

          if (criteria.StartDate != null && criteria.EndDate != null)
          {
            cmd.CommandText += @" AND (v.date >= @fromDate AND v.date < @toDate)";
            cmd.Parameters.AddWithValue("@fromDate", criteria.StartDate.Date);
            cmd.Parameters.AddWithValue("@toDate", criteria.EndDate.Date.AddDays(1));
          }

          cmd.CommandText += " GROUP BY v.vehiclefee,v.feeno,v.date,v.ppatotal";

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "v.date DESC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(VehicleFeeInfo.Get(dr));
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
