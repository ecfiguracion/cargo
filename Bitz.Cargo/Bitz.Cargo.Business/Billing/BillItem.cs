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
  #region BillItem
  [Serializable]
  public class BillItem : BusinessBase<BillItem>
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

    public static readonly PropertyInfo<int> _Bill = RegisterProperty<int>(c => c.Bill, "BillItem");
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

    #region UnitCount

    public static readonly PropertyInfo<int?> _UnitCount = RegisterProperty<int?>(c => c.UnitCount);
    public int? UnitCount
    {
      get { return GetProperty(_UnitCount); }
      set { SetProperty(_UnitCount, value); }
    }

    #endregion

    #region Uom

    public static readonly PropertyInfo<int?> _Uom = RegisterProperty<int?>(c => c.Uom);
    public int? Uom
    {
      get { return GetProperty(_Uom); }
      set 
      {
        this.QtyConversion = 0;
        GetUomConversion(value);
        SetProperty(_Uom, value);
        
      }
    }

    #endregion

    #region QtyConversion

    public static readonly PropertyInfo<decimal> _QtyConversion = RegisterProperty<decimal>(c => c.QtyConversion);
    public decimal QtyConversion
    {
      get { return GetProperty(_QtyConversion); }
      set { SetProperty(_QtyConversion, value); }
    }

    #endregion

    #region WeightUsed

    public static readonly PropertyInfo<int?> _WeightUsed = RegisterProperty<int?>(c => c.WeightUsed);
    public int? WeightUsed
    {
      get { return GetProperty(_WeightUsed); }
      set 
      {
        GetRate();
        SetProperty(_WeightUsed, value);
      }
    }

    #endregion

    #region StevedoringRate

    public static readonly PropertyInfo<decimal> _StevedoringRate = RegisterProperty<decimal>(c => c.StevedoringRate);
    public decimal StevedoringRate
    {
      get { return GetProperty(_StevedoringRate); }
      set { SetProperty(_StevedoringRate, value); }
    }

    #endregion

    #region StevedoringConst

    public static readonly PropertyInfo<decimal> _StevedoringConst = RegisterProperty<decimal>(c => c.StevedoringConst);
    public decimal StevedoringConst
    {
      get { return GetProperty(_StevedoringConst); }
      set { SetProperty(_StevedoringConst, value); }
    }

    #endregion

    #region ArrastreRate

    public static readonly PropertyInfo<decimal> _ArrastreRate = RegisterProperty<decimal>(c => c.ArrastreRate);
    public decimal ArrastreRate
    {
      get { return GetProperty(_ArrastreRate); }
      set { SetProperty(_ArrastreRate, value); }
    }

    #endregion

    #region ArrastreConst

    public static readonly PropertyInfo<decimal> _ArrastreConst = RegisterProperty<decimal>(c => c.ArrastreConst);
    public decimal ArrastreConst
    {
      get { return GetProperty(_ArrastreConst); }
      set { SetProperty(_ArrastreConst, value); }
    }

    #endregion

    #region PremiumRate

    public static readonly PropertyInfo<decimal> _PremiumRate = RegisterProperty<decimal>(c => c.PremiumRate);
    public decimal PremiumRate
    {
      get { return GetProperty(_PremiumRate); }
      set { SetProperty(_PremiumRate, value); }
    }

    #endregion


    #endregion

    #region Derived Properties

    #region StevedoringDisplayText
    public string StevedoringDisplayText
    {
      get
      {
        if ((int)this.WeightUsed == CargoConstants.WeightRates.MetricTons.Id)
        {
          return string.Format("{0:N2}", this.StevedoringRate);
        }
        else
        {
          return string.Format("({0:N3}) {1:N2}",this.StevedoringConst, this.StevedoringRate);
        }
      }
    }
    #endregion

    #region ArrastreDisplayText
    public string ArrastreDisplayText
    {
      get
      {
        if (this.WeightUsed == CargoConstants.WeightRates.MetricTons.Id)
        {
          return string.Format("{0:N2}", this.ArrastreRate);
        }
        else
        {
          return string.Format("({0:N3}) {1:N2}", this.ArrastreConst, this.ArrastreRate);
        }
      }
    }
    #endregion

    #region PremiumRateText
    public string PremiumRateText
    {
      get
      {
        return string.Format("{0:N0}%", this.PremiumRate);
      }
    }
    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Cargo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_UnitCount));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Uom));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_WeightUsed));
    }

    #endregion

    #region Factory Methods

    public static BillItem New()
    {
      return Csla.DataPortal.CreateChild<BillItem>();
    }

    public static BillItem Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BillItem>(dr);
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
      LoadProperty(_Id, dr.GetInt32("billitem"));
      LoadProperty(_Bill, dr.GetInt32("bill"));
      LoadProperty(_Cargo, BaseItemInfo.Get(dr, _Cargo.Name));
      LoadProperty(_UnitCount, dr.GetInt32("unitcount"));
      LoadProperty(_Uom, dr.GetInt32("uom"));
      LoadProperty(_WeightUsed, dr.GetInt32("weightused"));
      LoadProperty(_StevedoringRate, dr.GetDecimal("stevedoringrate"));
      LoadProperty(_StevedoringConst, dr.GetDecimal("stevedoringconst"));
      LoadProperty(_ArrastreRate, dr.GetDecimal("arrastrerate"));
      LoadProperty(_ArrastreConst, dr.GetDecimal("arrastreconst"));
      LoadProperty(_PremiumRate, dr.GetDecimal("premiumrate"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO billitem(bill,cargo,unitcount,uom,weightused,stevedoringrate,
                                  stevedoringconst,arrastrerate,arrastreconst,premiumrate,qtyconversion)
                              VALUES (@bill,@cargo,@unitcount,@uom,@weightused,@stevedoringrate,
                                  @stevedoringconst,@arrastrerate,@arrastreconst,@premiumrate,@qtyconversion)
                            SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@bill", parentId.Value);
          cmd.Parameters.AddWithValue("@cargo", Cargo.Id);
          cmd.Parameters.AddWithValue("@unitcount", UnitCount);
          cmd.Parameters.AddWithValue("@uom", Uom);
          cmd.Parameters.AddWithValue("@qtyconversion", QtyConversion);
          cmd.Parameters.AddWithValue("@weightused", WeightUsed);
          cmd.Parameters.AddWithValue("@stevedoringrate", StevedoringRate);
          cmd.Parameters.AddWithValue("@stevedoringconst", StevedoringConst);
          cmd.Parameters.AddWithValue("@arrastrerate", ArrastreRate);
          cmd.Parameters.AddWithValue("@arrastreconst", ArrastreConst);
          cmd.Parameters.AddWithValue("@premiumrate", PremiumRate);

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
          cmd.CommandText = @"UPDATE billitem SET 
                                cargo = @cargo,
                                unitcount = @unitcount,
                                uom = @uom,
                                qtyconversion = @qtyconversion,
                                weightused = @weightused,
                                stevedoringrate = @stevedoringrate,
                                stevedoringconst = @stevedoringconst,
                                arrastrerate = @arrastrerate,
                                arrastreconst = @arrastreconst,
                                premiumrate = @premiumrate
                              WHERE billitem = @id";
          cmd.Parameters.AddWithValue("@cargo", Cargo.Id);
          cmd.Parameters.AddWithValue("@unitcount", UnitCount);
          cmd.Parameters.AddWithValue("@uom", Uom);
          cmd.Parameters.AddWithValue("@qtyconversion", QtyConversion);
          cmd.Parameters.AddWithValue("@weightused", WeightUsed);
          cmd.Parameters.AddWithValue("@stevedoringrate", StevedoringRate);
          cmd.Parameters.AddWithValue("@stevedoringconst", StevedoringConst);
          cmd.Parameters.AddWithValue("@arrastrerate", ArrastreRate);
          cmd.Parameters.AddWithValue("@arrastreconst", ArrastreConst);
          cmd.Parameters.AddWithValue("@premiumrate", PremiumRate);
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
          cmd.CommandText = @"DELETE FROM billitem WHERE billitem = @id";
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

    #region GetRate

    private void GetRate()
    {
      if (this.Cargo == null) return;

      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT arrastremtrate,arrastrertrate,stevedoringrtrate,
                                  stevedoringmtrate, premiumrate,rtmultiplier
                              FROM item
                              WHERE item = @id";
          cmd.Parameters.AddWithValue("@id", this.Cargo.Id);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              var arrastremtrate = dr.GetDecimal("arrastremtrate");
              var arrastrertrate = dr.GetDecimal("arrastrertrate");
              var stevedoringmtrate = dr.GetDecimal("stevedoringmtrate");
              var stevedoringrtrate = dr.GetDecimal("stevedoringrtrate");
              var rtmultipler = dr.GetDecimal("rtmultiplier");
              var premiumrate = dr.GetDecimal("premiumrate");

              if (this.WeightUsed != null)
              {
                if ((int)this.WeightUsed == CargoConstants.WeightRates.MetricTons.Id)
                {
                  this.ArrastreRate = arrastremtrate;
                  this.StevedoringRate = stevedoringmtrate;
                  this.StevedoringConst = 0;
                  this.ArrastreConst = 0;
                }
                else
                {
                  this.ArrastreRate = arrastrertrate;
                  this.ArrastreConst = rtmultipler;
                  this.StevedoringRate = stevedoringrtrate;
                  this.StevedoringConst = rtmultipler;
                }
                OnPropertyChanged("ArrastreDisplayText");
                OnPropertyChanged("StevedoringDisplayText");
              }
              this.PremiumRate = premiumrate;
            }
          }
        }
      }
    }

    private void GetUomConversion(int? Uom)
    {
      if (this.Cargo == null && Uom == null) return;

      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT quantity
                              FROM itemuomconversion
                              WHERE item = @id AND uom = @uom";
          cmd.Parameters.AddWithValue("@id", this.Cargo.Id);
          cmd.Parameters.AddWithValue("@uom", Uom);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              this.QtyConversion = dr.GetDecimal("quantity");
            }
          }
        }
      }
    }

    #endregion

    #endregion
  }
  #endregion

  #region BillItems
  [Serializable]
  public class BillItems : BusinessListBase<BillItems, BillItem>
  {
    #region Factory Methods

    public static BillItems New()
    {
      return new BillItems();
    }

    public static BillItems Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<BillItems>(id);
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
                              SELECT bi.billitem,bi.bill,bi.cargo,bi.unitcount,bi.uom,bi.weightused,bi.stevedoringrate,
                                  bi.stevedoringconst,bi.arrastrerate,bi.arrastreconst,bi.premiumrate,bi.qtyconversion,
                                  i.item as {0}item,i.itemcode as {0}itemcode,i.itemname as {0}itemname    
                              FROM billitem bi
                              INNER JOIN item i ON bi.cargo = i.item
                              WHERE bi.bill = @id", "Cargo");
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(BillItem.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
