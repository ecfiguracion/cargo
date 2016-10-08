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

namespace Bitz.Cargo.Business.Items.Infos
{
  #region BaseItemInfo

  [Serializable]
  [TableInfo(TableName = "item", KeyColumn = "item.item")]
  public class BaseItemInfo : ReadOnlyBase<BaseItemInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Code

    public static readonly PropertyInfo<string> _Code = RegisterProperty<string>(c => c.Code);
    public string Code
    {
      get { return GetProperty(_Code); }
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

    #region Derived Properties

    public string DisplayText
    {
      get
      {
        return string.Format("{0} - {1}", this.Code, this.Name);
      }
    }

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static BaseItemInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BaseItemInfo>(dr,string.Empty);
    }

    public static BaseItemInfo Get(SafeDataReader dr, string columnprefix)
    {
      return Csla.DataPortal.FetchChild<BaseItemInfo>(dr, columnprefix);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<BaseItemInfo>> completed)
    {
      DataPortal<BaseItemInfo> dp = new DataPortal<BaseItemInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr,string columnprefix)
    {
      LoadProperty(_Id, dr.GetInt32(columnprefix + "item"));
      LoadProperty(_Code, dr.GetString(columnprefix + "itemcode"));
      LoadProperty(_Name, dr.GetString(columnprefix + "itemname"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM item WHERE item = @id";
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

  #region BaseItemInfos

  [Serializable]
  public class BaseItemInfos : ReadOnlyListBase<BaseItemInfos, BaseItemInfo>
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

      #region ItemType
      //private static PropertyInfo<int?> _ItemType = RegisterProperty<int?>(c => c.ItemType);

      //public int? ItemType
      //{
      //  get { return ReadProperty(_ItemType); }
      //  set { LoadProperty(_ItemType, value); }
      //}
      #endregion //ItemType

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<BaseItemInfos>> completed)
    {
      DataPortal<BaseItemInfos> dp = new DataPortal<BaseItemInfos>();
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
          cmd.CommandText = @"SELECT item,itemcode,itemname
                              FROM item
                              WHERE (itemcode LIKE @SearchText OR itemname LIKE @SearchText)";
          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "itemname ASC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(BaseItemInfo.Get(dr));
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
