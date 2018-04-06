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

namespace Bitz.Cargo.Business.Disbursements
{
  #region DisbursementItem
  [Serializable]
  public class DisbursementItem : BusinessBase<DisbursementItem>
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

    #region Disbursement

    public static readonly PropertyInfo<int> _Disbursement = RegisterProperty<int>(c => c.Disbursement);
    public int Disbursement
    {
      get { return GetProperty(_Disbursement); }
      set { SetProperty(_Disbursement, value); }
    }

    #endregion

    #region Particulars

    public static readonly PropertyInfo<string> _Particulars = RegisterProperty<string>(c => c.Particulars, "Particulars");
    public string Particulars
    {
      get { return GetProperty(_Particulars); }
      set { SetProperty(_Particulars, value); }
    }

    #endregion

    #region Amount

    public static readonly PropertyInfo<decimal?> _Amount = RegisterProperty<decimal?>(c => c.Amount);
    public decimal? Amount
    {
      get { return GetProperty(_Amount); }
      set { SetProperty(_Amount, value); }
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Particulars));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Amount));
    }

    #endregion

    #region Factory Methods

    public static DisbursementItem New()
    {
      return Csla.DataPortal.CreateChild<DisbursementItem>();
    }

    public static DisbursementItem Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<DisbursementItem>(dr);
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
      LoadProperty(_Id, dr.GetInt32("disbursementitem"));
      LoadProperty(_Disbursement, dr.GetString("disbursement"));
      LoadProperty(_Particulars, dr.GetString("particulars"));
      LoadProperty(_Amount, dr.GetDecimal("amount"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO disbursementitem(disbursement,particulars,amount)
                              VALUES (@disbursement,@particulars,@amount)
                            SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@disbursement", parentId.Value);
          cmd.Parameters.AddWithValue("@particulars", Particulars);
          cmd.Parameters.AddWithValue("@amount", Amount);

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
          cmd.CommandText = @"UPDATE disbursementitem SET 
                                disbursement = @disbursement,
                                particulars = @particulars,
                                amount = @amount                    
                              WHERE disbursementitem = @id";
          cmd.Parameters.AddWithValue("@disbursement", parentId.Value);
          cmd.Parameters.AddWithValue("@particulars", Particulars);
          cmd.Parameters.AddWithValue("@amount", Amount);
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
          cmd.CommandText = @"DELETE FROM disbursementitem WHERE disbursementitem = @id";
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

  #region DisbursementItems
  [Serializable]
  public class DisbursementItems : BusinessListBase<DisbursementItems, DisbursementItem>
  {
    #region Factory Methods

    public static DisbursementItems New()
    {
      return new DisbursementItems();
    }

    public static DisbursementItems Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<DisbursementItems>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT disbursementitem,disbursement,particulars,amount
                              FROM disbursementitem
                              WHERE disbursement = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(DisbursementItem.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
