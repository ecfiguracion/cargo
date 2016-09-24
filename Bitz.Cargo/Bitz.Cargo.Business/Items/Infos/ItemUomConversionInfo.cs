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
  #region ItemUomConversionInfo

  [Serializable]
  [TableInfo(TableName = "itemuomconversion", KeyColumn = "itemuomconversion.itemuomconversion")]
  public class ItemUomConversionInfo : ReadOnlyBase<ItemUomConversionInfo>
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

    public static readonly PropertyInfo<int> _Item = RegisterProperty<int>(c => c.Item);
    public int Item
    {
      get { return GetProperty(_Item); }
    }

    #endregion

    #region Uom

    public static readonly PropertyInfo<int> _Uom = RegisterProperty<int>(c => c.Uom);
    public int Uom
    {
      get { return GetProperty(_Uom); }
    }

    #endregion


    #region Quantity

    public static readonly PropertyInfo<decimal> _Quantity = RegisterProperty<decimal>(c => c.Quantity);
    public decimal Quantity
    {
      get { return GetProperty(_Quantity); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static ItemUomConversionInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ItemUomConversionInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<ItemUomConversionInfo>> completed)
    {
      DataPortal<ItemUomConversionInfo> dp = new DataPortal<ItemUomConversionInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("itemuomconversion"));
      LoadProperty(_Item, dr.GetInt32("item"));
      LoadProperty(_Uom, dr.GetInt32("uom"));
      LoadProperty(_Quantity, dr.GetDecimal("quantity"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM itemuomconversion WHERE itemuomconversion = @id";
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

  #region ItemUomConversionInfos

  [Serializable]
  public class ItemUomConversionInfos : ReadOnlyListBase<ItemUomConversionInfos, ItemUomConversionInfo>
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

      #region Item
      private static PropertyInfo<int> _Item = RegisterProperty<int>(c => c.Item);

      public int Item
      {
        get { return ReadProperty(_Item); }
        set { LoadProperty(_Item, value); }
      }
      #endregion //Item

      #region Uom
      private static PropertyInfo<int> _Uom = RegisterProperty<int>(c => c.Uom);

      public int Uom
      {
        get { return ReadProperty(_Uom); }
        set { LoadProperty(_Uom, value); }
      }
      #endregion //Uom

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<ItemUomConversionInfos>> completed)
    {
      DataPortal<ItemUomConversionInfos> dp = new DataPortal<ItemUomConversionInfos>();
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
          cmd.CommandText = @"SELECT itemuomconversion,item,uom,quantity
                              FROM itemuomconversion
                              WHERE 1 = 1";
          //cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          if (criteria.Item != null && criteria.Item > 0)
            cmd.CommandText += " AND item = " + criteria.Item.ToString();

          if (criteria.Uom != null && criteria.Uom > 0)
            cmd.CommandText += " AND uom = " + criteria.Uom.ToString();

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(ItemUomConversionInfo.Get(dr));
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
