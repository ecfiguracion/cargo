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

namespace Bitz.Cargo.Business.Items
{
  #region ItemInfo

  [Serializable]
  [TableInfo(TableName = "item", KeyColumn = "item.item")]
  public class ItemInfo : ReadOnlyBase<ItemInfo>
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

    #region ShortDescription

    public static readonly PropertyInfo<string> _ShortDescription = RegisterProperty<string>(c => c.ShortDescription);
    public string ShortDescription
    {
      get { return GetProperty(_ShortDescription); }
    }
    #endregion

    #region Remarks

    public static readonly PropertyInfo<string> _Remarks = RegisterProperty<string>(c => c.Remarks);
    public string Remarks
    {
      get { return GetProperty(_Remarks); }
    }

    #endregion

    #region ItemTypeName

    public static readonly PropertyInfo<string> _ItemTypeName = RegisterProperty<string>(c => c.ItemTypeName);
    public string ItemTypeName
    {
      get { return GetProperty(_ItemTypeName); }
    }

    #endregion

    #region ItemType

    public static readonly PropertyInfo<int?> _ItemType = RegisterProperty<int?>(c => c.ItemType);
    public int? ItemType
    {
      get { return GetProperty(_ItemType); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static ItemInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ItemInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<ItemInfo>> completed)
    {
      DataPortal<ItemInfo> dp = new DataPortal<ItemInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("item"));
      LoadProperty(_Code, dr.GetString("itemid"));
      LoadProperty(_Name, dr.GetString("itemname"));
      LoadProperty(_ShortDescription, dr.GetString("shortdescription"));
      LoadProperty(_Remarks, dr.GetString("remarks"));
      LoadProperty(_ItemType, dr.GetInt16("itemtype"));
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

  #region ItemInfos

  [Serializable]
  public class ItemInfos : ReadOnlyListBase<ItemInfos, ItemInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : CriteriaBase<Criteria>
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
      private static PropertyInfo<int?> _ItemType = RegisterProperty<int?>(c => c.ItemType);

      public int? ItemType
      {
        get { return ReadProperty(_ItemType); }
        set { LoadProperty(_ItemType, value); }
      }
      #endregion //ItemType

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<ItemInfos>> completed)
    {
      DataPortal<ItemInfos> dp = new DataPortal<ItemInfos>();
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
          cmd.CommandText = @"SELECT item,itemid,itemname,shortdescription,remarks,itemtype
                              FROM item
                              WHERE (itemid LIKE @SearchText 
                                      OR itemname LIKE @SearchText
                                      OR remarks LIKE @SearchText
                                      OR shortdescription LIKE @SearchText)
                              ORDER BY itemname";
          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          //if (criteria.ItemType != null)
          //  cmd.Parameters.AddWithValue("@itemtype", criteria.ItemType);
          //else
          //  cmd.Parameters.AddWithValue("@itemtype", DBNull.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(ItemInfo.Get(dr));
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
