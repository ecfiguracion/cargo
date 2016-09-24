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
  #region ItemRateInfo

  [Serializable]
  [TableInfo(TableName = "itemprice", KeyColumn = "itemprice.itemprice")]
  public class ItemRateInfo : ReadOnlyBase<ItemRateInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Item

    public static readonly PropertyInfo<int?> _Item = RegisterProperty<int?>(c => c.Item);
    public int? Item
    {
      get { return GetProperty(_Item); }
    }
    #endregion

    #region UnitName

    public static readonly PropertyInfo<string> _UnitName = RegisterProperty<string>(c => c.UnitName);
    public string UnitName
    {
      get { return GetProperty(_UnitName); }
    }

    #endregion

    #region ItemRate

    public static readonly PropertyInfo<decimal> _ItemRate = RegisterProperty<decimal>(c => c.ItemRate);
    public decimal ItemRate
    {
      get { return GetProperty(_ItemRate); }
    }

    #endregion

    #region ChargeType

    public static readonly PropertyInfo<string> _ChargeType = RegisterProperty<string>(c => c.ChargeType);
    public string ChargeType
    {
      get { return GetProperty(_ChargeType); }
    }

    #endregion

    #region ConstantValue

    public static readonly PropertyInfo<decimal> _ConstantValue = RegisterProperty<decimal>(c => c.ConstantValue);
    public decimal ConstantValue
    {
      get { return GetProperty(_ConstantValue); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static ItemRateInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ItemRateInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<ItemRateInfo>> completed)
    {
      DataPortal<ItemRateInfo> dp = new DataPortal<ItemRateInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("itemprice"));
      LoadProperty(_Item, dr.GetInt32("item"));
      LoadProperty(_UnitName, dr.GetString("unitname"));
      LoadProperty(_ItemRate, dr.GetDecimal("unitprice"));
      LoadProperty(_ChargeType, dr.GetString("chargetype"));
      LoadProperty(_ConstantValue, dr.GetDecimal("quantity"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM itemrate WHERE itemrate = @id";
          cm.Parameters.AddWithValue("@id", id.Value);
          cm.ExecuteNonQuery();
        }
      }
    }

    #endregion

    #endregion
  }

  #endregion

  #region ItemRateInfos

  [Serializable]
  public class ItemRateInfos : ReadOnlyListBase<ItemRateInfos, ItemRateInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : CriteriaBase<Criteria>
    {
      #region ItemId
      private static PropertyInfo<int> _ItemId = RegisterProperty<int>(c => c.ItemId);

      public int ItemId
      {
        get { return ReadProperty(_ItemId); }
        set { LoadProperty(_ItemId, value); }
      }
      #endregion //SearchText

      #region ExcludeStringIds
      private static PropertyInfo<string> _ExcludeStringIds = RegisterProperty<string>(c => c.ExcludeStringIds);

      public string ExcludeStringIds
      {
        get { return ReadProperty(_ExcludeStringIds); }
        set { LoadProperty(_ExcludeStringIds, value); }
      }
      #endregion
    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<ItemRateInfos>> completed)
    {
      DataPortal<ItemRateInfos> dp = new DataPortal<ItemRateInfos>();
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
          cmd.CommandText = @"select ip.itemprice, ip.item, uom.name as unitname, ip.unitprice, ip.quantity,
	                              case when ip.classification = 0 then 'Arrastre'
		                              when ip.classification = 1 then 'Stevedoring'
		                              when ip.classification = 2 then 'Roll-on/Roll-off' end as chargetype
                              from itemprice ip
	                              left join uom on uom.uom = ip.uom
                              WHERE item = @item";
          cmd.Parameters.AddWithValue("@item", criteria.ItemId.ToString());

          if (!string.IsNullOrEmpty(criteria.ExcludeStringIds))
          {
            cmd.CommandText += @" AND ip.itemprice NOT IN (" + criteria.ExcludeStringIds + ")";
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(ItemRateInfo.Get(dr));
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
