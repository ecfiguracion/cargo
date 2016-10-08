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
  public class ItemUomRate : BusinessBase<ItemUomRate>
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

    #region Rate

    public static readonly PropertyInfo<decimal?> _Rate = RegisterProperty<decimal?>(c => c.Rate);
    public decimal? Rate
    {
      get { return GetProperty(_Rate); }
      set { SetProperty(_Rate, value); }
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Rate));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Uom));
    }

    #endregion

    #region Factory Methods

    public static ItemUomRate New()
    {
      return Csla.DataPortal.CreateChild<ItemUomRate>();
    }

    public static ItemUomRate Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ItemUomRate>(dr);
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
      LoadProperty(_Id, dr.GetInt32("itemuomrate"));
      LoadProperty(_Item, dr.GetInt32("item"));
      LoadProperty(_Rate, dr.GetDecimal("rate"));
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
          cmd.CommandText = @"INSERT INTO ItemUomRate(item,rate,uom)
                                        VALUES (@item,@rate,@uom)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@item", parentId.Value);
          cmd.Parameters.AddWithValue("@rate", Rate);
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
          cmd.CommandText = @"UPDATE itemuomrate SET
                                            item = @item,
                                            rate = @rate,
                                            uom = @uom
                                        WHERE ItemUomRate = @id";
          cmd.Parameters.AddWithValue("@item", parentId.Value);
          cmd.Parameters.AddWithValue("@rate", Rate);
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
          cmd.CommandText = @"DELETE FROM itemuomrate WHERE itemuomrate = @id";
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
  public class ItemUomRates : BusinessListBase<ItemUomRates, ItemUomRate>
  {
    #region Factory Methods

    public static ItemUomRates New()
    {
      return new ItemUomRates();
    }

    public static ItemUomRates Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<ItemUomRates>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT itemuomrate,item,rate,uom
                                        FROM itemuomrate
                                        WHERE item = @item";
          cmd.Parameters.AddWithValue("@item", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(ItemUomRate.Get(dr));
            }
          }
        }
      }

    }


    #endregion
  }

}
