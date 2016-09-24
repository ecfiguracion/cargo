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
  [Serializable]
  public class ItemUomConversion : BusinessBase<ItemUomConversion>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
      set { SetProperty(_Id, value); }
    }

    #endregion

    #region Item

    public static readonly PropertyInfo<int> _Item = RegisterProperty<int>(c => c.Item);
    public int Item
    {
      get { return GetProperty(_Item); }
      set { SetProperty(_Item, value); }
    }

    #endregion

    #region Quantity

    public static readonly PropertyInfo<decimal?> _Quantity = RegisterProperty<decimal?>(c => c.Quantity);
    public decimal? Quantity
    {
      get { return GetProperty(_Quantity); }
      set { SetProperty(_Quantity, value); }
    }

    #endregion

    #region Uom

    public static readonly PropertyInfo<int?> _Uom = RegisterProperty<int?>(c => c.Uom);
    public int? Uom
    {
      get { return GetProperty(_Uom); }
      set { SetProperty(_Uom, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Quantity));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Uom));
    }

    #endregion

    #region Factory Methods

    public static ItemUomConversion New()
    {
      return Csla.DataPortal.CreateChild<ItemUomConversion>();
    }

    public static ItemUomConversion Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ItemUomConversion>(dr);
    }

    #endregion

    #region Data Access

    #region Fetch

    protected override void Child_Create()
    {
      base.Child_Create();
      BusinessRules.CheckRules();
    }

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("itemuomconversion"));
      LoadProperty(_Item, dr.GetInt32("item"));
      LoadProperty(_Quantity, dr.GetDecimal("quantity"));
      LoadProperty(_Uom, dr.GetInt32("uom"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO itemuomconversion(item,quantity,uom)
                                        VALUES (@item,@quantity,@uom)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@item", parentId.Value);
          cmd.Parameters.AddWithValue("@quantity", Quantity);
          cmd.Parameters.AddWithValue("@uom", Uom);
          try
          {
            int identity = Convert.ToInt32(cmd.ExecuteScalar());
            LoadProperty(_Id, identity);
          }
          catch (Exception)
          {
            throw;
          }
        }
      }

    }

    #endregion

    #region Child Update

    protected void Child_Update(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE itemuomconversion SET
                                            item = @item,
                                            quantity = @quantity,
                                            uom = @uom
                                        WHERE itemuomconversion = @id";
          cmd.Parameters.AddWithValue("@item", parentId.Value);
          cmd.Parameters.AddWithValue("@quantity", Quantity);
          cmd.Parameters.AddWithValue("@uom", Uom);
          cmd.Parameters.AddWithValue("@id", this.Id);

          try
          {
            cmd.ExecuteNonQuery();
          }
          catch (Exception)
          {
            throw;
          }
        }
      }

    }

    #endregion

    #region Child Delete

    protected void Child_DeleteSelf()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"DELETE FROM itemuomconversion WHERE itemuomconversion = @id";
          cmd.Parameters.AddWithValue("@id", this.Id);

          try
          {
            cmd.ExecuteNonQuery();
          }
          catch (Exception)
          {
            throw;
          }
        }
      }

    }

    #endregion

    #endregion
  }

  [Serializable]
  public class ItemUomConversions : BusinessListBase<ItemUomConversions, ItemUomConversion>
  {
    #region Factory Methods

    public static ItemUomConversions New()
    {
      return new ItemUomConversions();
    }

    public static ItemUomConversions Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<ItemUomConversions>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT itemuomconversion,item,quantity,uom
                                        FROM itemuomconversion
                                        WHERE item = @item";
          cmd.Parameters.AddWithValue("@item", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(ItemUomConversion.Get(dr));
            }
          }
        }
      }

    }


    #endregion
  }

}
