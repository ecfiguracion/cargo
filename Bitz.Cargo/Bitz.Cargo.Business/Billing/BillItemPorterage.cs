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
  #region BillItemPorterage
  [Serializable]
  public class BillItemPorterage : BusinessBase<BillItemPorterage>
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

    public static readonly PropertyInfo<int> _Bill = RegisterProperty<int>(c => c.Bill, "BillItemPorterage");
    public int Bill
    {
      get { return GetProperty(_Bill); }
      set { SetProperty(_Bill, value); }
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
          this.Total = ((decimal)value * this.Rate) / (decimal)1.12;
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
          this.Total = ((decimal)this.Quantity * value) / (decimal)1.12;
        }
      }
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Remarks));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Quantity));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Rate));
    }

    #endregion

    #region Factory Methods

    public static BillItemPorterage New()
    {
      return Csla.DataPortal.CreateChild<BillItemPorterage>();
    }

    public static BillItemPorterage Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillItemPorterage>(dr);
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
      LoadProperty(_Id, dr.GetInt32("billitemporterage"));
      LoadProperty(_Bill, dr.GetInt32("bill"));
      LoadProperty(_Quantity, dr.GetInt32("Quantity"));
      LoadProperty(_Rate, dr.GetDecimal("rate"));
      LoadProperty(_Remarks, dr.GetString("remarks"));
      LoadProperty(_Total, (decimal)(Quantity * Rate) / (decimal)1.12);
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO BillItemPorterage(bill,remarks,quantity,rate)
                              VALUES (@bill,@remarks,@quantity,@rate)
                            SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@bill", parentId.Value);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@quantity", Quantity);
          cmd.Parameters.AddWithValue("@rate", Rate);

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
          cmd.CommandText = @"UPDATE BillItemPorterage SET 
                                quantity = @quantity,
                                remarks = @remarks,
                                rate = @rate
                              WHERE BillItemPorterage = @id";
          cmd.Parameters.AddWithValue("@quantity", Quantity);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@rate", Rate);
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
          cmd.CommandText = @"DELETE FROM BillItemPorterage WHERE BillItemPorterage = @id";
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

    #endregion
  }
  #endregion

  #region BillItemPorterages
  [Serializable]
  public class BillItemPorterages : BusinessListBase<BillItemPorterages, BillItemPorterage>
  {
    #region Factory Methods

    public static BillItemPorterages New()
    {
      return new BillItemPorterages();
    }

    public static BillItemPorterages Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<BillItemPorterages>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT billitemporterage,bill,remarks,quantity,rate
                              FROM billitemporterage
                              WHERE bill = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(BillItemPorterage.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
