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
  #region ItemRate
  [Serializable]
  public class ItemRate : BusinessBase<ItemRate>
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

    public static readonly PropertyInfo<int?> _Item = RegisterProperty<int?>(c => c.Item, "Item");
    public int? Item
    {
      get { return GetProperty(_Item); }
      set { SetProperty(_Item, value); }
    }

    #endregion

    #region ItemUnit

    public static readonly PropertyInfo<int?> _ItemUnit = RegisterProperty<int?>(c => c.ItemUnit, "Item Unit");
    public int? ItemUnit
    {
      get { return GetProperty(_ItemUnit); }
      set { SetProperty(_ItemUnit, value); }
    }

    #endregion

    #region Rate

    public static readonly PropertyInfo<decimal?> _Rate = RegisterProperty<decimal?>(c => c.Rate, "Rate");
    public decimal? Rate
    {
      get { return GetProperty(_Rate); }
      set { SetProperty(_Rate, value); }
    }

    #endregion

    #region ChargeType

    public static readonly PropertyInfo<int?> _ChargeType = RegisterProperty<int?>(c => c.ChargeType, "Charge Type");
    public int? ChargeType
    {
      get { return GetProperty(_ChargeType); }
      set { SetProperty(_ChargeType, value); }
    }
    #endregion

    #region Remarks

    public static readonly PropertyInfo<string> _Remarks = RegisterProperty<string>(c => c.Remarks, "Remarks");
    public string Remarks
    {
      get { return GetProperty(_Remarks); }
      set { SetProperty(_Remarks, value); }
    }

    #endregion

    public static readonly PropertyInfo<decimal?> _ConstantValue = RegisterProperty<decimal?>(c => c.ConstantValue, "Constant Value");
    public decimal? ConstantValue
    {
      get { return GetProperty(_ConstantValue); }
      set { SetProperty(_ConstantValue, value); }
    }

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ItemUnit));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Rate));
      //BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ChargeType));
    }

    #endregion

    #region Factory Methods

    public static ItemRate New()
    {
      return Csla.DataPortal.CreateChild<ItemRate>();
    }

    public static ItemRate Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ItemRate>(dr);
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
      LoadProperty(_Id, dr.GetInt32("itemprice"));
      LoadProperty(_Item, dr.GetInt32("item"));
      LoadProperty(_ItemUnit, dr.GetInt32("uom"));
      LoadProperty(_Rate, dr.GetDecimal("unitprice"));
      LoadProperty(_ChargeType, dr.GetInt16("classification"));
      LoadProperty(_Remarks, dr.GetString("remarks"));
      LoadProperty(_ConstantValue, dr.GetDecimal("quantity"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO itemprice (item,classification,uom,unitprice,remarks,quantity)
                              VALUES (@item,@classification,@uom,@unitprice,@remarks,@constantvalue)
                                        SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@item", parentId.Value);
          cmd.Parameters.AddWithValue("@uom", ItemUnit);
          cmd.Parameters.AddWithValue("@unitprice", Rate);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          if (ChargeType != null)
            cmd.Parameters.AddWithValue("@classification", ChargeType);
          else
            cmd.Parameters.AddWithValue("@classification", DBNull.Value);

          if (ConstantValue != null)
            cmd.Parameters.AddWithValue("@constantvalue", ConstantValue);
          else
            cmd.Parameters.AddWithValue("@constantvalue", DBNull.Value);

          try
          {
            int identity = Convert.ToInt32(cmd.ExecuteScalar());
            LoadProperty(_Id, identity);
          }
          catch (Exception e)
          {
            throw e;
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
          cmd.CommandText = @"UPDATE itemprice
                               SET item = @item
                                  ,classification = @classification
                                  ,uom = @uom
                                  ,unitprice = @unitprice
                                  ,remarks = @remarks
                                  ,quantity = @constantvalue
                                WHERE itemprice = @id";

          cmd.Parameters.AddWithValue("@item", parentId.Value);
          cmd.Parameters.AddWithValue("@classification", ChargeType);
          cmd.Parameters.AddWithValue("@uom", ItemUnit);
          cmd.Parameters.AddWithValue("@unitprice", Rate);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@constantvalue", ConstantValue);
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
          cmd.CommandText = @"DELETE FROM itemprice WHERE itemprice = @id";
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
  #endregion

  #region ItemRates
  [Serializable]
  public class ItemRates : BusinessListBase<ItemRates, ItemRate>
  {
    #region Factory Methods

    public static ItemRates New()
    {
      return new ItemRates();
    }

    public static ItemRates Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<ItemRates>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT itemprice
                              ,item
                              ,classification
                              ,uom
                              ,unitprice
                              ,quantity
                              ,remarks
                          FROM itemprice
                          WHERE item = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(ItemRate.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
