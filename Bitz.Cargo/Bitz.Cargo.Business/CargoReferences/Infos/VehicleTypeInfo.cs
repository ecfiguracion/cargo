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

namespace Bitz.Cargo.Business.CargoReferences.Infos
{
  #region VehicleTypeInfo

  [Serializable]
  [TableInfo(TableName = "vehicletype", KeyColumn = "vehicletype.vehicletype")]
  public class VehicleTypeInfo : ReadOnlyBase<VehicleTypeInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Type

    public static readonly PropertyInfo<int> _Type = RegisterProperty<int>(c => c.Type);
    public int Type
    {
      get { return GetProperty(_Type); }
    }

    #endregion

    #region Name

    public static readonly PropertyInfo<string> _Name = RegisterProperty<string>(c => c.Name);
    public string Name
    {
      get { return GetProperty(_Name); }
    }

    #endregion

    #region Fee

    public static readonly PropertyInfo<decimal> _Fee = RegisterProperty<decimal>(c => c.Fee);
    public decimal Fee
    {
      get { return GetProperty(_Fee); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static VehicleTypeInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<VehicleTypeInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<VehicleTypeInfo>> completed)
    {
      DataPortal<VehicleTypeInfo> dp = new DataPortal<VehicleTypeInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("vehicletype"));
      LoadProperty(_Type, dr.GetInt32("type"));
      LoadProperty(_Name, dr.GetString("name"));
      LoadProperty(_Fee, dr.GetDecimal("fee"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM vehicletype WHERE vehicletype = @id";
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

  #region VehicleTypeInfos

  [Serializable]
  public class VehicleTypeInfos : ReadOnlyListBase<VehicleTypeInfos, VehicleTypeInfo>
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

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<VehicleTypeInfos>> completed)
    {
      DataPortal<VehicleTypeInfos> dp = new DataPortal<VehicleTypeInfos>();
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
          cmd.CommandText = @"SELECT vehicletype,type,name,fee
                              FROM vehicletype
                              WHERE (name LIKE @SearchText)";
          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "name ASC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(VehicleTypeInfo.Get(dr));
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
