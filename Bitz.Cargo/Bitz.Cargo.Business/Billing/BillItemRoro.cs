using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Billing
{
  #region BillItemRoro
  [Serializable]
  public class BillItemRoro : BusinessBase<BillItemRoro>
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

    #region Bill

    public static readonly PropertyInfo<int> _Bill = RegisterProperty<int>(c => c.Bill, "BillItemRoro");
    public int Bill
    {
      get { return GetProperty(_Bill); }
      set { SetProperty(_Bill, value); }
    }

    #endregion

    #region Cargo

    public static readonly PropertyInfo<BaseItemInfo> _Cargo = RegisterProperty<BaseItemInfo>(c => c.Cargo, "Cargo");
    public BaseItemInfo Cargo
    {
      get { return GetProperty(_Cargo); }
      set { SetProperty(_Cargo, value); }
    }

    #endregion

    #region Uom

    public static readonly PropertyInfo<int?> _Uom = RegisterProperty<int?>(c => c.Uom);
    public int? Uom
    {
      get { return GetProperty(_Uom); }
      set 
      {
        this.Rate = 0;
        GetRate(value);
        SetProperty(_Uom, value);
      }
    }

    #endregion

    #region Quantity

    public static readonly PropertyInfo<int?> _Quantity = RegisterProperty<int?>(c => c.Quantity);
    public int? Quantity
    {
      get { return GetProperty(_Quantity); }
      set 
      { 
        SetProperty(_Quantity, value);
        if (value.HasValue && this.Rate > 0)
        {
          this.Total = (decimal)value * this.Rate;
        }
      }
    }

    #endregion

    #region Rate

    public static readonly PropertyInfo<decimal> _Rate = RegisterProperty<decimal>(c => c.Rate);
    public decimal Rate
    {
      get { return GetProperty(_Rate); }
      set 
      { 
        SetProperty(_Rate, value);
        if (this.Quantity.HasValue && this.Rate > 0)
        {
          this.Total = (decimal)this.Quantity * value;
        }
      }
    }

    #endregion

    #region IsTaxable

    public static readonly PropertyInfo<bool> _IsTaxable = RegisterProperty<bool>(c => c.IsTaxable);
    public bool IsTaxable
    {
      get { return GetProperty(_IsTaxable); }
      set { SetProperty(_IsTaxable, value); }
    }

    #endregion

    #region Total

    public static readonly PropertyInfo<decimal> _Total = RegisterProperty<decimal>(c => c.Total);
    public decimal Total
    {
      get { return GetProperty(_Total); }
      set { SetProperty(_Total, value); }
    }

    #endregion

    #endregion

    #region Derived Properties

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Cargo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Quantity));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Uom));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Rate));
    }

    #endregion

    #region Factory Methods

    public static BillItemRoro New()
    {
      return Csla.DataPortal.CreateChild<BillItemRoro>();
    }

    public static BillItemRoro Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillItemRoro>(dr);
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
      LoadProperty(_Id, dr.GetInt32("billitemroro"));
      LoadProperty(_Bill, dr.GetInt32("bill"));
      LoadProperty(_Quantity, dr.GetInt32("Quantity"));
      LoadProperty(_Uom, dr.GetInt32("uom"));
      LoadProperty(_Cargo, BaseItemInfo.Get(dr, _Cargo.Name));
      LoadProperty(_Rate, dr.GetDecimal("rate"));
      LoadProperty(_IsTaxable, dr.GetBoolean("istaxable"));
      LoadProperty(_Total, this.Quantity * this.Rate);
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO BillItemRoro(bill,cargo,quantity,uom,rate,istaxable)
                              VALUES (@bill,@cargo,@quantity,@uom,@rate,@istaxable)
                            SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@bill", parentId.Value);
          cmd.Parameters.AddWithValue("@cargo", Cargo.Id);
          cmd.Parameters.AddWithValue("@quantity", Quantity);
          cmd.Parameters.AddWithValue("@uom", Uom);
          cmd.Parameters.AddWithValue("@rate", Rate);
          cmd.Parameters.AddWithValue("@istaxable", IsTaxable);

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
          cmd.CommandText = @"UPDATE BillItemRoro SET 
                                cargo = @cargo,
                                quantity = @quantity,
                                uom = @uom,
                                rate = @rate,
                                istaxable = @istaxable
                              WHERE BillItemRoro = @id";
          cmd.Parameters.AddWithValue("@cargo", Cargo.Id);
          cmd.Parameters.AddWithValue("@quantity", Quantity);
          cmd.Parameters.AddWithValue("@uom", Uom);
          cmd.Parameters.AddWithValue("@rate", Rate);
          cmd.Parameters.AddWithValue("@istaxable", IsTaxable);
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
          cmd.CommandText = @"DELETE FROM BillItemRoro WHERE BillItemRoro = @id";
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

    #region Methods

    private void GetRate(int? Uom)
    {
      if (this.Cargo == null && Uom != null) return;

      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT rate FROM itemuomrate
                              WHERE item = @id AND uom = @uom";
          cmd.Parameters.AddWithValue("@id", this.Cargo.Id);
          cmd.Parameters.AddWithValue("@uom", Uom);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              this.Rate = dr.GetDecimal("rate");
            }
          }
        }
      }
    }

    #endregion
  }
  #endregion

  #region BillItemRoros
  [Serializable]
  public class BillItemRoros : BusinessListBase<BillItemRoros, BillItemRoro>
  {
    #region Factory Methods

    public static BillItemRoros New()
    {
      return new BillItemRoros();
    }

    public static BillItemRoros Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<BillItemRoros>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = string.Format(@"
                              SELECT bi.billitemroro,bi.bill,bi.cargo,bi.quantity,bi.uom,bi.rate,bi.istaxable,
                                  i.item as {0}item,i.itemcode as {0}itemcode,i.itemname as {0}itemname    
                              FROM billitemroro bi
                              INNER JOIN item i ON bi.cargo = i.item
                              WHERE bi.bill = @id", "Cargo");
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(BillItemRoro.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
