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
  public class Item : BusinessBase<Item>
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

    #region Code

    public static readonly PropertyInfo<string> _Code = RegisterProperty<string>(c => c.Code, "Code");
    public string Code
    {
      get { return GetProperty(_Code); }
      set { SetProperty(_Code, value); }
    }

    #endregion

    #region Name

    public static readonly PropertyInfo<string> _Name = RegisterProperty<string>(c => c.Name, "Name");
    public string Name
    {
      get { return GetProperty(_Name); }
      set { SetProperty(_Name, value); }
    }
    #endregion

    #region RevenueMultiplier

    public static readonly PropertyInfo<decimal?> _RevenueMultiplier = RegisterProperty<decimal?>(c => c.RevenueMultiplier, "Revenue Multiplier");
    public decimal? RevenueMultiplier
    {
      get { return GetProperty(_RevenueMultiplier); }
      set { SetProperty(_RevenueMultiplier, value); }
    }
    #endregion

    #region HandlingUnit

    public static readonly PropertyInfo<int?> _HandlingUnit = RegisterProperty<int?>(c => c.HandlingUnit, "Handling Unit");
    public int? HandlingUnit
    {
      get { return GetProperty(_HandlingUnit); }
      set { SetProperty(_HandlingUnit, value); }
    }

    #endregion

    #region ArrMetricRate

    public static readonly PropertyInfo<decimal?> _ArrMetricRate = RegisterProperty<decimal?>(c => c.ArrMetricRate, "Arrastre Metric Rate");
    public decimal? ArrMetricRate
    {
      get { return GetProperty(_ArrMetricRate); }
      set { SetProperty(_ArrMetricRate, value); }
    }
    #endregion

    #region ArrRevenueRate

    public static readonly PropertyInfo<decimal?> _ArrRevenueRate = RegisterProperty<decimal?>(c => c.ArrRevenueRate, "Arrastre Revenue Rate");
    public decimal? ArrRevenueRate
    {
      get { return GetProperty(_ArrRevenueRate); }
      set { SetProperty(_ArrRevenueRate, value); }
    }
    #endregion

    #region StevMetricRate

    public static readonly PropertyInfo<decimal?> _StevMetricRate = RegisterProperty<decimal?>(c => c.StevMetricRate, "Stevedoring Metric Rate");
    public decimal? StevMetricRate
    {
      get { return GetProperty(_StevMetricRate); }
      set { SetProperty(_StevMetricRate, value); }
    }
    #endregion

    #region StevRevenueRate

    public static readonly PropertyInfo<decimal?> _StevRevenueRate = RegisterProperty<decimal?>(c => c.StevRevenueRate, "Stevedoring Revenue Rate");
    public decimal? StevRevenueRate
    {
      get { return GetProperty(_StevRevenueRate); }
      set { SetProperty(_StevRevenueRate, value); }
    }
    #endregion

    #endregion

    #region One To Many Properties

    #region ItemUomConversions

    public static readonly PropertyInfo<ItemUomConversions> _ItemUomConversions = RegisterProperty<ItemUomConversions>(c => c.ItemUomConversions);
    public ItemUomConversions ItemUomConversions
    {
      get { return GetProperty(_ItemUomConversions); }
      set { SetProperty(_ItemUomConversions, value); }
    }

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Name));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_HandlingUnit));
    }

    #endregion
    
    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Item>> completed)
    {
      DataPortal<Item> dp = new DataPortal<Item>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Item>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      this.BusinessRules.CheckRules();
    }

    #endregion

    #region Fetch

    private void DataPortal_Fetch(int id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT item
                              ,itemid
                              ,itemname
                              ,rtconstant
                              ,handlingunit
                              ,mtrate1
                              ,mtrate2
                              ,rtrate1
                              ,rtrate2
                              ,uom.name as handlingunitname
                          FROM item
                            LEFT JOIN uom ON uom.uom = item.handlingunit
                          WHERE item = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("item"));
              LoadProperty(_Code, dr.GetString("itemid"));
              LoadProperty(_Name, dr.GetString("itemname"));
              LoadProperty(_RevenueMultiplier, dr.GetDecimal("rtconstant"));
              LoadProperty(_HandlingUnit, dr.GetInt32("handlingunit"));
              LoadProperty(_ArrMetricRate, dr.GetDecimal("mtrate1"));
              LoadProperty(_ArrRevenueRate, dr.GetDecimal("rtrate1"));
              LoadProperty(_StevMetricRate, dr.GetDecimal("mtrate2"));
              LoadProperty(_StevRevenueRate, dr.GetDecimal("rtrate2"));
            }
          }
        }
      }
      ChildFetch();
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO item (itemid,itemname,rtconstant,handlingunit,mtrate1,mtrate2,rtrate1,rtrate2)
                              VALUES (@itemid,@itemname,@rtconstant,@handlingunit,@mtrate1,@mtrate2,@rtrate1,@rtrate2)
                                        SELECT SCOPE_IDENTITY()";

          //if (ItemType != null)
          //  cmd.Parameters.AddWithValue("@itemtype", ItemType);
          //else
          //  cmd.Parameters.AddWithValue("@itemtype", DBNull.Value);

          cmd.Parameters.AddWithValue("@itemid", Code);
          cmd.Parameters.AddWithValue("@itemname", Name);
          cmd.Parameters.AddWithValue("@rtconstant", RevenueMultiplier);
          cmd.Parameters.AddWithValue("@handlingunit", HandlingUnit);
          cmd.Parameters.AddWithValue("@mtrate1", ArrMetricRate);
          cmd.Parameters.AddWithValue("@mtrate2", StevMetricRate);
          cmd.Parameters.AddWithValue("@rtrate1", ArrRevenueRate);
          cmd.Parameters.AddWithValue("@rtrate2", StevRevenueRate);

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
      SaveChild();
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE item
                               SET itemid = @code
                                  ,itemname = @name
                                  ,rtconstant = @rtconstant
                                  ,handlingunit = @handlingunit
                                  ,mtrate1 = @mtrate1
                                  ,mtrate2 = @mtrate2
                                  ,rtrate1 = @rtrate1
                                  ,rtrate2 = @rtrate2
                                WHERE item = @id";

          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@handlingunit", HandlingUnit);
          cmd.Parameters.AddWithValue("@rtconstant", RevenueMultiplier);
          cmd.Parameters.AddWithValue("@mtrate1", ArrMetricRate);
          cmd.Parameters.AddWithValue("@mtrate2", StevMetricRate);
          cmd.Parameters.AddWithValue("@rtrate1", ArrRevenueRate);
          cmd.Parameters.AddWithValue("@rtrate2", StevRevenueRate);
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
      SaveChild();
    }

    #endregion

    #region ChildFetch

    private void ChildFetch()
    {
      LoadProperty(_ItemUomConversions, ItemUomConversions.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region SaveChild

    private void SaveChild()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_ItemUomConversions), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #endregion
  }
}
