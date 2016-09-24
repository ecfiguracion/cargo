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
  #region BillRate
  [Serializable]
  public class BillRate : BusinessBase<BillRate>
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

    #region BillingItem

    public static readonly PropertyInfo<int?> _BillingItem = RegisterProperty<int?>(c => c.BillingItem, "BillingItem");
    public int? BillingItem
    {
      get { return GetProperty(_BillingItem); }
      set { SetProperty(_BillingItem, value); }
    }

    #endregion

    #region ItemRate

    public static readonly PropertyInfo<int?> _ItemRate = RegisterProperty<int?>(c => c.ItemRate, "ItemRate");
    public int? ItemRate
    {
      get { return GetProperty(_ItemRate); }
      set { SetProperty(_ItemRate, value); }
    }

    #endregion

    #region ItemRateHandling

    public static readonly PropertyInfo<string> _ItemRateHandling = RegisterProperty<string>(c => c.ItemRateHandling, "Handling Item Rate");
    public string ItemRateHandling
    {
      get { return GetProperty(_ItemRateHandling); }
      set { SetProperty(_ItemRateHandling, value); }
    }

    #endregion

    #region ItemRateUnit

    public static readonly PropertyInfo<string> _ItemRateUnit = RegisterProperty<string>(c => c.ItemRateUnit, "Item Rate Unit");
    public string ItemRateUnit
    {
      get { return GetProperty(_ItemRateUnit); }
      set { SetProperty(_ItemRateUnit, value); }
    }

    #endregion

    #region Computation1

    public static readonly PropertyInfo<decimal?> _Computation1 = RegisterProperty<decimal?>(c => c.Computation1);
    public decimal? Computation1
    {
      get { return GetProperty(_Computation1); }
      set { SetProperty(_Computation1, value); }
    }
    #endregion

    #region Computation2

    public static readonly PropertyInfo<decimal?> _Computation2 = RegisterProperty<decimal?>(c => c.Computation2);
    public decimal? Computation2
    {
      get { return GetProperty(_Computation2); }
      set { SetProperty(_Computation2, value); }
    }
    #endregion

    #region Computation3

    public static readonly PropertyInfo<decimal?> _Computation3 = RegisterProperty<decimal?>(c => c.Computation3);
    public decimal? Computation3
    {
      get { return GetProperty(_Computation3); }
      set { SetProperty(_Computation3, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      //BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ItemUnit));
      //BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Rate));
      //BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ChargeType));
    }

    #endregion

    #region Factory Methods

    public static BillRate New()
    {
      return Csla.DataPortal.CreateChild<BillRate>();
    }

    public static BillRate Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillRate>(dr);
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
      LoadProperty(_Id, dr.GetInt32("BillRate"));
      LoadProperty(_ItemRate, dr.GetInt32("itemrate"));
      LoadProperty(_BillingItem, dr.GetInt32("billingitem"));
      LoadProperty(_ItemRateHandling, dr.GetString("chargetype"));
      LoadProperty(_Computation1, dr.GetDecimal("computation1"));
      LoadProperty(_Computation2, dr.GetDecimal("computation2"));
      LoadProperty(_Computation3, dr.GetDecimal("computation3"));
      LoadProperty(_ItemRateUnit, dr.GetString("unitname"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO BillRate (itemrate,billingitem,computation1)
                              VALUES (@itemprice,@billingitem,@computation1)
                                        SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@billingitem", parentId.Value);
          cmd.Parameters.AddWithValue("@itemprice", ItemRate);

          if (Computation1 != null)
            cmd.Parameters.AddWithValue("@computation1", Computation1);
          else
            cmd.Parameters.AddWithValue("@computation1", DBNull.Value);

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
          cmd.CommandText = @"UPDATE BillRate
                               SET itemrate = @itemrate
                                  ,billingitem = @billingitem
                                  ,computation1 = @computation1
                                WHERE BillRate = @id";

          cmd.Parameters.AddWithValue("@billingitem", parentId.Value);
          cmd.Parameters.AddWithValue("@itemrate", ItemRate);
          cmd.Parameters.AddWithValue("@computation1", Computation1);
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
          cmd.CommandText = @"DELETE FROM BillRate WHERE BillRate = @id";
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

  #region BillRates
  [Serializable]
  public class BillRates : BusinessListBase<BillRates, BillRate>
  {
    #region Factory Methods

    public static BillRates New()
    {
      return new BillRates();
    }

    public static BillRates Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<BillRates>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"select bi.billingitem, 
	                            br.billitemrate, 
	                            ip.itemprice as itemrate, 
	                            case when ip.classification = 0 then 'Arrastre'
		                            when ip.classification = 1 then 'Stevedoring'
		                            when ip.classification = 2 then 'Roll-on/Roll-off' end as chargetype,
	                            uom.name as unitname, 
	                            br.computation1,
	                            ip.quantity as computation2,
	                            ip.unitprice as computation3
                            from billingitem bi
                              inner join billitemrate br on br.billingitem = bi.billingitem
                              left join itemprice ip on ip.itemprice = br.itemrate
                              left join uom on uom.uom = ip.uom
                              left join itemuomconversion on itemuomconversion.item = bi.item
	                              and itemuomconversion.uom = bi.preferreduom
                            where bi.billingitem = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(BillRate.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
