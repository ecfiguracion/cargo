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

    public static readonly PropertyInfo<string> _ShortDescription = RegisterProperty<string>(c => c.ShortDescription, "Short Description");
    public string ShortDescription
    {
      get { return GetProperty(_ShortDescription); }
      set { SetProperty(_ShortDescription, value); }
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

    #region HandlingUnit

    public static readonly PropertyInfo<int?> _HandlingUnit = RegisterProperty<int?>(c => c.HandlingUnit, "Handling Unit");
    public int? HandlingUnit
    {
      get { return GetProperty(_HandlingUnit); }
      set { SetProperty(_HandlingUnit, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #region ItemUnitRates

    public static readonly PropertyInfo<ItemRates> _ItemUnitRates = RegisterProperty<ItemRates>(c => c.ItemUnitRates);
    public ItemRates ItemUnitRates
    {
      get { return GetProperty(_ItemUnitRates); }
      set { SetProperty(_ItemUnitRates, value); }
    }
    #endregion

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
      //LoadProperty(_ItemType, 2);
      //LoadProperty(_ItemUnitRates, ItemRates.New());
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
                              ,shortdescription
                              ,remarks
                              ,handlingunit
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
              LoadProperty(_ShortDescription, dr.GetString("shortdescription"));
              LoadProperty(_Remarks, dr.GetString("remarks"));
              LoadProperty(_HandlingUnit, dr.GetInt32("handlingunit"));
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
          cmd.CommandText = @"INSERT INTO item (itemid,itemname,shortdescription,remarks,handlingunit)
                              VALUES (@itemid,@itemname,@shortdescription,@remarks,@handlingunit)
                                        SELECT SCOPE_IDENTITY()";

          //if (ItemType != null)
          //  cmd.Parameters.AddWithValue("@itemtype", ItemType);
          //else
          //  cmd.Parameters.AddWithValue("@itemtype", DBNull.Value);

          cmd.Parameters.AddWithValue("@itemid", Code);
          cmd.Parameters.AddWithValue("@itemname", Name);
          cmd.Parameters.AddWithValue("@shortdescription", ShortDescription);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@handlingunit", HandlingUnit);

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
                                  ,shortdescription = @shortdescription
                                  ,remarks = @remarks
                                  ,handlingunit = @handlingunit
                                WHERE item = @id";

          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@handlingunit", HandlingUnit);
          cmd.Parameters.AddWithValue("@shortdescription", ShortDescription);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
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
      LoadProperty(_ItemUnitRates, ItemRates.Get(new SingleCriteria<int>(this.Id)));
      LoadProperty(_ItemUomConversions, ItemUomConversions.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region SaveChild

    private void SaveChild()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_ItemUnitRates), new SingleCriteria<int>(this.Id));
      Csla.DataPortal.UpdateChild(ReadProperty(_ItemUomConversions), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #endregion
  }
}
