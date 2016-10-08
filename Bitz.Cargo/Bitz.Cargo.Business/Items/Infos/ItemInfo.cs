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

    #region ArrastreMTRate

    public static readonly PropertyInfo<decimal> _ArrastreMTRate = RegisterProperty<decimal>(c => c.ArrastreMTRate);
    public decimal ArrastreMTRate
    {
      get { return GetProperty(_ArrastreMTRate); }
    }

    #endregion

    #region ArrastreRTRate

    public static readonly PropertyInfo<decimal> _ArrastreRTRate = RegisterProperty<decimal>(c => c.ArrastreRTRate);
    public decimal ArrastreRTRate
    {
      get { return GetProperty(_ArrastreRTRate); }
    }

    #endregion

    #region StevedoringMTRate

    public static readonly PropertyInfo<decimal> _StevedoringMTRate = RegisterProperty<decimal>(c => c.StevedoringMTRate);
    public decimal StevedoringMTRate
    {
      get { return GetProperty(_StevedoringMTRate); }
    }

    #endregion

    #region StevedoringRTRate

    public static readonly PropertyInfo<decimal> _StevedoringRTRate = RegisterProperty<decimal>(c => c.StevedoringRTRate);
    public decimal StevedoringRTRate
    {
      get { return GetProperty(_StevedoringRTRate); }
    }

    #endregion

    #region RTMultiplier

    public static readonly PropertyInfo<decimal> _RTMultiplier = RegisterProperty<decimal>(c => c.RTMultiplier);
    public decimal RTMultiplier
    {
      get { return GetProperty(_RTMultiplier); }
    }

    #endregion

    #region PremiumRate

    public static readonly PropertyInfo<decimal> _PremiumRate = RegisterProperty<decimal>(c => c.PremiumRate);
    public decimal PremiumRate
    {
      get { return GetProperty(_PremiumRate); }
    }

    #endregion

    #region IsTaxWithHeld

    public static readonly PropertyInfo<bool> _IsTaxWithHeld = RegisterProperty<bool>(c => c.IsTaxWithHeld);
    public bool IsTaxWithHeld
    {
      get { return GetProperty(_IsTaxWithHeld); }
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
      LoadProperty(_Code, dr.GetString("itemcode"));
      LoadProperty(_Name, dr.GetString("itemname"));
      LoadProperty(_ShortDescription, dr.GetString("shortdescription"));
      LoadProperty(_ArrastreMTRate, dr.GetDecimal("arrastremtrate"));
      LoadProperty(_ArrastreRTRate, dr.GetDecimal("arrastrertrate"));
      LoadProperty(_StevedoringMTRate, dr.GetDecimal("stevedoringmtrate"));
      LoadProperty(_StevedoringRTRate, dr.GetDecimal("stevedoringrtrate"));
      LoadProperty(_PremiumRate, dr.GetDecimal("premiumrate"));
      LoadProperty(_RTMultiplier, dr.GetDecimal("rtmultiplier"));
      LoadProperty(_IsTaxWithHeld, dr.GetBoolean("istaxwithheld"));
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
          cmd.CommandText = @"SELECT item,itemcode,itemname,shortdescription,arrastremtrate,arrastrertrate,premiumrate,
                                    stevedoringmtrate,stevedoringrtrate,rtmultiplier,istaxwithheld
                              FROM item                              
                              WHERE (itemname LIKE @SearchText OR itemcode LIKE @SearchText)";
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
