using Bitz.Business.Settings.Infos;
using Bitz.Core.Constants;
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

    #region ShortDescription

    public static readonly PropertyInfo<string> _ShortDescription = RegisterProperty<string>(c => c.ShortDescription);
    public string ShortDescription
    {
      get { return GetProperty(_ShortDescription); }
      set { SetProperty(_ShortDescription, value); }
    }
    #endregion

    #region ArrastreMTRate

    public static readonly PropertyInfo<decimal> _ArrastreMTRate = RegisterProperty<decimal>(c => c.ArrastreMTRate);
    public decimal ArrastreMTRate
    {
      get { return GetProperty(_ArrastreMTRate); }
      set { SetProperty(_ArrastreMTRate, value); }
    }

    #endregion

    #region ArrastreRTRate

    public static readonly PropertyInfo<decimal> _ArrastreRTRate = RegisterProperty<decimal>(c => c.ArrastreRTRate);
    public decimal ArrastreRTRate
    {
      get { return GetProperty(_ArrastreRTRate); }
      set { SetProperty(_ArrastreRTRate, value); }
    }

    #endregion

    #region StevedoringMTRate

    public static readonly PropertyInfo<decimal> _StevedoringMTRate = RegisterProperty<decimal>(c => c.StevedoringMTRate);
    public decimal StevedoringMTRate
    {
      get { return GetProperty(_StevedoringMTRate); }
      set { SetProperty(_StevedoringMTRate, value); }
    }

    #endregion

    #region StevedoringRTRate

    public static readonly PropertyInfo<decimal> _StevedoringRTRate = RegisterProperty<decimal>(c => c.StevedoringRTRate);
    public decimal StevedoringRTRate
    {
      get { return GetProperty(_StevedoringRTRate); }
      set { SetProperty(_StevedoringRTRate, value); }
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

    #region RTMultiplier

    public static readonly PropertyInfo<decimal> _RTMultiplier = RegisterProperty<decimal>(c => c.RTMultiplier);
    public decimal RTMultiplier
    {
      get { return GetProperty(_RTMultiplier); }
      set { SetProperty(_RTMultiplier, value); }
    }

    #endregion

    #region IsTaxWithHeld

    public static readonly PropertyInfo<bool> _IsTaxWithHeld = RegisterProperty<bool>(c => c.IsTaxWithHeld);
    public bool IsTaxWithHeld
    {
      get { return GetProperty(_IsTaxWithHeld); }
      set { SetProperty(_IsTaxWithHeld, value); }
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Name, 300));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Code));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Code,50));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_ShortDescription, 30));
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
      LoadProperty(_Code, "[Auto-Assign]");
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
          cmd.CommandText = @"SELECT item,itemcode,itemname,shortdescription,arrastremtrate,arrastrertrate,premiumrate,
                                    stevedoringmtrate,stevedoringrtrate,rtmultiplier,istaxwithheld
                          FROM item
                          WHERE item = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("item"));
              LoadProperty(_Code, dr.GetString("itemcode"));
              LoadProperty(_Name, dr.GetString("itemname"));
              LoadProperty(_ShortDescription, dr.GetString("shortdescription"));
              LoadProperty(_ArrastreMTRate, dr.GetDecimal("arrastremtrate"));
              LoadProperty(_ArrastreRTRate, dr.GetDecimal("arrastrertrate"));
              LoadProperty(_StevedoringMTRate, dr.GetDecimal("stevedoringmtrate"));
              LoadProperty(_StevedoringRTRate, dr.GetDecimal("stevedoringrtrate"));
              LoadProperty(_PremiumRate, dr.GetDecimal("premiumrate"));
              LoadProperty(_RTMultiplier, dr.GetDecimal("rtmultiplier"));
              LoadProperty(_IsTaxWithHeld, dr.GetBoolean("istaxwithheld"));
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
          cmd.CommandText = @"INSERT INTO item(itemcode,itemname,shortdescription,arrastremtrate,arrastrertrate,
                                  stevedoringmtrate,stevedoringrtrate,rtmultiplier,premiumrate,istaxwithheld)
                              VALUES (@itemcode,@itemname,@shortdescription,@arrastremtrate,@arrastrertrate,
                                  @stevedoringmtrate,@stevedoringrtrate,@rtmultiplier,@premiumrate,@istaxwithheld)
                              SELECT SCOPE_IDENTITY()";
          LoadProperty(_Code, string.Format("CRG{0:yy}{0:MM}{1:0000}", DateTime.Now.Date,
              TableCounterInfo.Get(new SingleCriteria<int>(BitzConstants.TableCounter.Item.Id)).Counter));
          cmd.Parameters.AddWithValue("@itemcode", Code);
          cmd.Parameters.AddWithValue("@itemname", Name);
          cmd.Parameters.AddWithValue("@shortdescription", ShortDescription);
          cmd.Parameters.AddWithValue("@arrastremtrate", ArrastreMTRate);
          cmd.Parameters.AddWithValue("@arrastrertrate", ArrastreRTRate);
          cmd.Parameters.AddWithValue("@stevedoringmtrate", StevedoringMTRate);
          cmd.Parameters.AddWithValue("@stevedoringrtrate", StevedoringRTRate);
          cmd.Parameters.AddWithValue("@rtmultiplier", RTMultiplier);
          cmd.Parameters.AddWithValue("@premiumrate", PremiumRate);
          cmd.Parameters.AddWithValue("@istaxwithheld", IsTaxWithHeld);
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
          cmd.CommandText = @"UPDATE item SET
                                  itemcode = @itemcode,
                                  itemname = @itemname,
                                  shortdescription = @shortdescription,
                                  arrastremtrate = @arrastremtrate,
                                  arrastrertrate = @arrastrertrate,
                                  stevedoringmtrate = @stevedoringmtrate,
                                  stevedoringrtrate = @stevedoringrtrate,
                                  rtmultiplier = @rtmultiplier,
                                  premiumrate = @premiumrate,
                                  istaxwithheld = @istaxwithheld
                                WHERE item = @id";
          cmd.Parameters.AddWithValue("@itemcode", Code);
          cmd.Parameters.AddWithValue("@itemname", Name);
          cmd.Parameters.AddWithValue("@shortdescription", ShortDescription);
          cmd.Parameters.AddWithValue("@arrastremtrate", ArrastreMTRate);
          cmd.Parameters.AddWithValue("@arrastrertrate", ArrastreRTRate);
          cmd.Parameters.AddWithValue("@stevedoringmtrate", StevedoringMTRate);
          cmd.Parameters.AddWithValue("@stevedoringrtrate", StevedoringRTRate);
          cmd.Parameters.AddWithValue("@rtmultiplier", RTMultiplier);
          cmd.Parameters.AddWithValue("@premiumrate", PremiumRate);
          cmd.Parameters.AddWithValue("@istaxwithheld", IsTaxWithHeld);
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
