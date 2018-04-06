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
  #region VoucherTypeInfo

  [Serializable]
  [TableInfo(TableName = "vouchertype", KeyColumn = "vouchertype.vouchertype")]
  public class VoucherTypeInfo : ReadOnlyBase<VoucherTypeInfo>
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

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static VoucherTypeInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<VoucherTypeInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<VoucherTypeInfo>> completed)
    {
      DataPortal<VoucherTypeInfo> dp = new DataPortal<VoucherTypeInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      Child_Fetch(dr, string.Empty);
    }

    private void Child_Fetch(SafeDataReader dr, string columnprefix)
    {
      LoadProperty(_Id, dr.GetInt32(columnprefix + "vouchertype"));
      LoadProperty(_Type, dr.GetInt32(columnprefix + "type"));
      LoadProperty(_Name, dr.GetString(columnprefix + "name"));
    }


    public static VoucherTypeInfo Get(SafeDataReader dr, string columnprefix)
    {
      return Csla.DataPortal.FetchChild<VoucherTypeInfo>(dr, columnprefix);
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM vouchertype WHERE vouchertype = @id";
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

  #region VoucherTypeInfos

  [Serializable]
  public class VoucherTypeInfos : ReadOnlyListBase<VoucherTypeInfos, VoucherTypeInfo>
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

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<VoucherTypeInfos>> completed)
    {
      DataPortal<VoucherTypeInfos> dp = new DataPortal<VoucherTypeInfos>();
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
          cmd.CommandText = @"SELECT vouchertype,type,name
                              FROM vouchertype
                              WHERE (name LIKE @SearchText)";
          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "vouchertype ASC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(VoucherTypeInfo.Get(dr));
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
