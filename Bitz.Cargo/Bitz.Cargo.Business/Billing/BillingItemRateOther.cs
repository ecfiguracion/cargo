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
  #region BillingItemRateOther
  [Serializable]
  public class BillingItemRateOther : BusinessBase<BillingItemRateOther>
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

    #region Percentage

    public static readonly PropertyInfo<int?> _Percentage = RegisterProperty<int?>(c => c.Percentage);
    public int? Percentage
    {
      get { return GetProperty(_Percentage); }
      set { SetProperty(_Percentage, value); }
    }
    #endregion

    #region FixedAmount

    public static readonly PropertyInfo<decimal?> _FixedAmount = RegisterProperty<decimal?>(c => c.FixedAmount);
    public decimal? FixedAmount
    {
      get { return GetProperty(_FixedAmount); }
      set { SetProperty(_FixedAmount, value); }
    }
    #endregion

    #region Description

    public static readonly PropertyInfo<string> _Description = RegisterProperty<string>(c => c.Description);
    public string Description
    {
      get { return GetProperty(_Description); }
      set { SetProperty(_Description, value); }
    }

    #endregion

    #region RateType

    public static readonly PropertyInfo<int> _RateType = RegisterProperty<int>(c => c.RateType);
    public int RateType
    {
      get { return GetProperty(_RateType); }
      set { SetProperty(_RateType, value); }
    }
    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Description));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_RateType));
    }

    #endregion

    #region Factory Methods

    public static BillingItemRateOther New()
    {
      return Csla.DataPortal.CreateChild<BillingItemRateOther>();
    }

    public static BillingItemRateOther Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillingItemRateOther>(dr);
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
      LoadProperty(_Id, dr.GetInt32("billingitemrateothers"));
      LoadProperty(_Percentage, dr.GetInt16("otherspercentage"));
      LoadProperty(_FixedAmount, dr.GetDecimal("othersfixedamount"));
      LoadProperty(_Description, dr.GetString("othersdescription"));
      LoadProperty(_RateType, dr.GetInt16("ratetype"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO billingitemrateothers (billingitem,otherspercentage,othersfixedamount,othersdescription,ratetype)
                              VALUES (@billingitem,@otherspercentage,@othersfixedamount,@othersdescription,@ratetype)
                                        SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@billingitem", parentId.Value);
          if (Percentage != null)
            cmd.Parameters.AddWithValue("@otherspercentage", Percentage);
          else
            cmd.Parameters.AddWithValue("@otherspercentage", DBNull.Value);

          if (FixedAmount != null)
            cmd.Parameters.AddWithValue("@othersfixedamount", FixedAmount);
          else
            cmd.Parameters.AddWithValue("@othersfixedamount", DBNull.Value);

          cmd.Parameters.AddWithValue("@othersdescription", Description);
          cmd.Parameters.AddWithValue("@ratetype", RateType);

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
          cmd.CommandText = @"UPDATE billingitemrateothers
                               SET billingitem = @billingitem
                                  ,otherspercentage = @otherspercentage
                                  ,othersfixedamount = @othersfixedamount
                                  ,othersdescription = @othersdescription
                                  ,ratetype = @ratetype
                                WHERE billingitemrateothers = @id";

          cmd.Parameters.AddWithValue("@billingitem", parentId.Value);
          if (Percentage != null)
            cmd.Parameters.AddWithValue("@otherspercentage", Percentage);
          else
            cmd.Parameters.AddWithValue("@otherspercentage", DBNull.Value);

          if (FixedAmount != null)
            cmd.Parameters.AddWithValue("@othersfixedamount", FixedAmount);
          else
            cmd.Parameters.AddWithValue("@othersfixedamount", DBNull.Value);

          cmd.Parameters.AddWithValue("@othersdescription", Description);
          cmd.Parameters.AddWithValue("@ratetype", RateType);
          cmd.Parameters.AddWithValue("@id", this.Id);
          try
          {
            cmd.ExecuteNonQuery();
          }
          catch (Exception e)
          {
            throw e;
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
          cmd.CommandText = @"DELETE FROM billingitemrateothers WHERE billingitemrateothers = @id";
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

  #region BillingItemRateOthers
  [Serializable]
  public class BillingItemRateOthers : BusinessListBase<BillingItemRateOthers, BillingItemRateOther>
  {
    #region Factory Methods

    public static BillingItemRateOthers New()
    {
      return new BillingItemRateOthers();
    }

    public static BillingItemRateOthers Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<BillingItemRateOthers>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"select billingitemrateothers,billingitem,otherspercentage,othersfixedamount,othersdescription,ratetype
                              from billingitemrateothers
                            where billingitem = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(BillingItemRateOther.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
