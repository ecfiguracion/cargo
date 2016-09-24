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
  [Serializable]
  public class Roro : BusinessBase<Roro>
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

    #region BillingItemType

    public static readonly PropertyInfo<int> _BillingItemType = RegisterProperty<int>(c => c.BillingItemType, "Type");
    public int BillingItemType
    {
      get { return GetProperty(_BillingItemType); }
      set { SetProperty(_BillingItemType, value); }
    }

    #endregion

    #region ReferenceNo

    public static readonly PropertyInfo<string> _ReferenceNo = RegisterProperty<string>(c => c.ReferenceNo, "Reference No.");
    public string ReferenceNo
    {
      get { return GetProperty(_ReferenceNo); }
      set { SetProperty(_ReferenceNo, value); }
    }

    #endregion

    #region BillingDate

    public static readonly PropertyInfo<SmartDate> _BillingDate = RegisterProperty<SmartDate>(c => c.BillingDate, "Billing Date");
    public SmartDate BillingDate
    {
      get { return GetProperty(_BillingDate); }
      set { SetProperty(_BillingDate, value); }
    }

    #endregion

    #region BillLadingNo

    public static readonly PropertyInfo<string> _BillLadingNo = RegisterProperty<string>(c => c.BillLadingNo, "Bill Lading No.");
    public string BillLadingNo
    {
      get { return GetProperty(_BillLadingNo); }
      set { SetProperty(_BillLadingNo, value); }
    }

    #endregion

    #region Consignee

    public static readonly PropertyInfo<int?> _Consignee = RegisterProperty<int?>(c => c.Consignee);
    public int? Consignee
    {
      get { return GetProperty(_Consignee); }
      set { SetProperty(_Consignee, value); }
    }

    #endregion

    #region ConsigneeAddress

    public static readonly PropertyInfo<string> _ConsigneeAddress = RegisterProperty<string>(c => c.ConsigneeAddress, "Consignee Address");
    public string ConsigneeAddress
    {
      get { return GetProperty(_ConsigneeAddress); }
      set { SetProperty(_ConsigneeAddress, value); }
    }

    #endregion

    #region Vessel

    public static readonly PropertyInfo<int?> _Vessel = RegisterProperty<int?>(c => c.Vessel, "Vessel");
    public int? Vessel
    {
      get { return GetProperty(_Vessel); }
      set { SetProperty(_Vessel, value); }
    }

    #endregion

    #region VoyageNo

    public static readonly PropertyInfo<string> _VoyageNo = RegisterProperty<string>(c => c.VoyageNo, "Voyage No.");
    public string VoyageNo
    {
      get { return GetProperty(_VoyageNo); }
      set { SetProperty(_VoyageNo, value); }
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

    #region ItemCount

    public static readonly PropertyInfo<decimal> _ItemCount = RegisterProperty<decimal>(c => c.ItemCount, "Count");
    public decimal ItemCount
    {
      get { return GetProperty(_ItemCount); }
      set { SetProperty(_ItemCount, value); }
    }

    #endregion

    #region ItemUnit

    public static readonly PropertyInfo<int?> _ItemUnit = RegisterProperty<int?>(c => c.ItemUnit, "Unit");
    public int? ItemUnit
    {
      get { return GetProperty(_ItemUnit); }
      set { SetProperty(_ItemUnit, value); }
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

    #region UserAccount

    public static readonly PropertyInfo<int> _UserAccount = RegisterProperty<int>(c => c.UserAccount, "User Account");
    public int UserAccount
    {
      get { return GetProperty(_UserAccount); }
      set { SetProperty(_UserAccount, value); }
    }

    #region CreatedDate

    public static readonly PropertyInfo<SmartDate> _CreatedDate = RegisterProperty<SmartDate>(c => c.CreatedDate, "Created Date");
    public SmartDate CreatedDate
    {
      get { return GetProperty(_CreatedDate); }
      set { SetProperty(_CreatedDate, value); }
    }

    #endregion

    #region LastUpdatedDate

    public static readonly PropertyInfo<SmartDate> _LastUpdatedDate = RegisterProperty<SmartDate>(c => c.LastUpdatedDate, "Last Updated Date");
    public SmartDate LastUpdatedDate
    {
      get { return GetProperty(_LastUpdatedDate); }
      set { SetProperty(_LastUpdatedDate, value); }
    }

    #endregion

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Item));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_BillingDate));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_BillLadingNo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Consignee));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Vessel));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Item));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ItemCount));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ItemUnit));
      //BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_ItemName, 300));
    }

    #endregion
    
    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Roro>> completed)
    {
      DataPortal<Roro> dp = new DataPortal<Roro>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Roro>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_ReferenceNo, "[Auto-Number]");
      LoadProperty(_BillingDate, DateTime.Now);
      LoadProperty(_BillingItemType, 3);
      LoadProperty(_UserAccount, 1);
      LoadProperty(_CreatedDate, DateTime.Now);
      LoadProperty(_LastUpdatedDate, DateTime.Now);
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
          cmd.CommandText = @"SELECT billingitem,referenceno,billingdate,billladingno,customer,
	                              custpreferredaddress,vessel,voyageno,item,itemcount,preferreduom,preferreduom,remarks
                              FROM billingitem WHERE billingitem = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("billingitem"));
              LoadProperty(_ReferenceNo, dr.GetString("referenceno"));
              LoadProperty(_BillingDate, dr.GetSmartDate("billingdate"));
              LoadProperty(_BillLadingNo, dr.GetString("billladingno"));
              LoadProperty(_Consignee, dr.GetInt32("customer"));
              LoadProperty(_ConsigneeAddress, dr.GetString("custpreferredaddress"));
              LoadProperty(_Vessel, dr.GetInt32("vessel"));
              LoadProperty(_VoyageNo, dr.GetString("voyageno"));
              LoadProperty(_Item, dr.GetInt32("item"));
              LoadProperty(_ItemCount, dr.GetDecimal("itemcount"));
              LoadProperty(_ItemUnit, dr.GetInt32("preferreduom"));
              LoadProperty(_Remarks, dr.GetString("remarks"));
            }
          }
        }
      }

      this.ChildFetch();
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO billingitem(type,referenceno,billingdate,billladingno,customer,custpreferredaddress,
                                                         vessel,voyageno,item,itemcount,preferreduom,remarks,useraccount,createddate,lastupdateddate)
                                        VALUES (@type,@referenceno,@billingdate,@billladingno,@customer,@custpreferredaddress,
                                                         @vessel,@voyageno,@item,@itemcount,@uom,@remarks,@useraccount,@createddate,@lastupdateddate)
                                        SELECT SCOPE_IDENTITY()";
          LoadProperty(_ReferenceNo, DateTime.Now.ToString("yyyyMMdd-HHmmssff"));
          cmd.Parameters.AddWithValue("@type", BillingItemType);
          cmd.Parameters.AddWithValue("@referenceno", ReferenceNo);
          cmd.Parameters.AddWithValue("@billingdate", BillingDate.DBValue);
          cmd.Parameters.AddWithValue("@billladingno", BillLadingNo);
          cmd.Parameters.AddWithValue("@customer", Consignee);
          cmd.Parameters.AddWithValue("@custpreferredaddress", ConsigneeAddress);
          cmd.Parameters.AddWithValue("@vessel", Vessel);
          cmd.Parameters.AddWithValue("@voyageno", VoyageNo);
          cmd.Parameters.AddWithValue("@item", Item);
          cmd.Parameters.AddWithValue("@itemcount", ItemCount);
          cmd.Parameters.AddWithValue("@uom", ItemUnit);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@useraccount", UserAccount);
          cmd.Parameters.AddWithValue("@createddate", CreatedDate.DBValue);
          cmd.Parameters.AddWithValue("@lastupdateddate", LastUpdatedDate.DBValue);

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
      this.SaveChild();
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE billingitem SET 
                                            referenceno = @referenceno,
                                            billingdate = @billingdate,
                                            billladingno = @billladingno,
                                            customer = @customer,
                                            custpreferredaddress = @custpreferredaddress,
                                            vessel = @vessel,
                                            voyageno = @voyageno,
                                            item = @item,
                                            itemcount = @itemcount,
                                            preferreduom = @uom,
                                            remarks = @remarks,
                                            useraccount = @useraccount,
                                            lastupdateddate = @lastupdateddate
                                        WHERE billingitem = @id";

          cmd.Parameters.AddWithValue("@referenceno", ReferenceNo);
          cmd.Parameters.AddWithValue("@billingdate", BillingDate.DBValue);
          cmd.Parameters.AddWithValue("@billladingno", BillLadingNo);
          cmd.Parameters.AddWithValue("@customer", Consignee);
          cmd.Parameters.AddWithValue("@custpreferredaddress", ConsigneeAddress);
          cmd.Parameters.AddWithValue("@vessel", Vessel);
          cmd.Parameters.AddWithValue("@voyageno", VoyageNo);
          cmd.Parameters.AddWithValue("@item", Item);
          cmd.Parameters.AddWithValue("@itemcount", ItemCount);
          cmd.Parameters.AddWithValue("@uom", ItemUnit);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@useraccount", 1);
          cmd.Parameters.AddWithValue("@lastupdateddate", DateTime.Now);
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
      this.SaveChild();
    }

    #endregion

    #region ChildFetch

    private void ChildFetch()
    {
      //LoadProperty(_ItemPrices, ItemPrices.Get(new SingleCriteria<int>(this.Id)));
      //LoadProperty(_ItemUomConversions, ItemUomConversions.Get(new SingleCriteria<int>(this.Id)));
      //LoadProperty(_ItemInventory, ItemInventoryInfos.Get(new ItemInventoryInfos.Criteria() { Item = this.Id }));
      //LoadProperty(_ItemSuppliers, ItemSuppliers.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region SaveChild

    private void SaveChild()
    {
      //Csla.DataPortal.UpdateChild(ReadProperty(_ItemSim), new SingleCriteria<int>(this.Id));
      //Csla.DataPortal.UpdateChild(ReadProperty(_ItemStock), new SingleCriteria<int>(this.Id));
      //Csla.DataPortal.UpdateChild(ReadProperty(_ItemPrices), new SingleCriteria<int>(this.Id));
      //Csla.DataPortal.UpdateChild(ReadProperty(_ItemUomConversions), new SingleCriteria<int>(this.Id));
      //Csla.DataPortal.UpdateChild(ReadProperty(_ItemSuppliers), new SingleCriteria<int>(this.Id));
      //SaveItemInventoryLocation();
    }

    #endregion

    #region SaveItemInventoryLocation

    private void SaveItemInventoryLocation()
    {
//      if (this.ItemType != ItemTypes.Inventory.Key) return;

//      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
//      {
//        using (var cmd = ctx.Connection.CreateCommand())
//        {
//          cmd.CommandText = @"INSERT INTO iteminventory(item,itemlocation,qtyonhand,qtyonorder)
//                                        SELECT @item,l.location,0,0
//                                        FROM location l
//                                        WHERE NOT EXISTS (
//                                            SELECT 1 FROM iteminventory ii
//                                            WHERE ii.itemlocation = l.location
//                                            AND ii.item = @item)";
//          cmd.Parameters.AddWithValue("@item", Id);
//          try
//          {
//            cmd.ExecuteNonQuery();
//          }
//          catch (Exception)
//          {
//            throw;
//          }

//        }
//      }
    }

    #endregion

    #endregion
  }
}
