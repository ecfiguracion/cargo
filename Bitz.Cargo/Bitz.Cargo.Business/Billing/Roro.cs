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

    #region Cargo

    public static readonly PropertyInfo<int?> _Cargo = RegisterProperty<int?>(c => c.Cargo, "Cargo");
    public int? Cargo
    {
      get { return GetProperty(_Cargo); }
      set { SetProperty(_Cargo, value); }
    }

    #endregion

    #region ItemCount

    public static readonly PropertyInfo<decimal?> _ItemCount = RegisterProperty<decimal?>(c => c.ItemCount, "Count");
    public decimal? ItemCount
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

    #region ItemCountHandling

    public static readonly PropertyInfo<decimal?> _ItemCountHandling = RegisterProperty<decimal?>(c => c.ItemCountHandling, "Handling Unit Quantity");
    public decimal? ItemCountHandling
    {
      get { return GetProperty(_ItemCountHandling); }
      set
      {
        SetProperty(_ItemCountHandling, value);
        if (this.RoroHandlingRates != null && this.RoroHandlingRates.Any())
        {
          foreach (var item in RoroHandlingRates)
          {
            item.Computation1 = value;
          }
          if (value > 0)
            this.ComputeStatementOfAccount();
        }
      }
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

    #region GrossAmount

    public static readonly PropertyInfo<decimal?> _GrossAmount = RegisterProperty<decimal?>(c => c.GrossAmount);
    public decimal? GrossAmount
    {
      get { return GetProperty(_GrossAmount); }
      set { SetProperty(_GrossAmount, value); }
    }

    #endregion

    #region NetAmount

    public static readonly PropertyInfo<decimal?> _NetAmount = RegisterProperty<decimal?>(c => c.NetAmount);
    public decimal? NetAmount
    {
      get { return GetProperty(_NetAmount); }
      set { SetProperty(_NetAmount, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #region RoroHandlingRates

    public static readonly PropertyInfo<BillingItemRates> _RoroHandlingRates = RegisterProperty<BillingItemRates>(c => c.RoroHandlingRates);
    public BillingItemRates RoroHandlingRates
    {
      get { return GetProperty(_RoroHandlingRates); }
      set
      {
        SetProperty(_RoroHandlingRates, value);
        this.ComputeStatementOfAccount();
      }
    }

    public static readonly PropertyInfo<BillingItemRateOthers> _RoroHandlingRateOthers = RegisterProperty<BillingItemRateOthers>(c => c.RoroHandlingRateOthers);
    public BillingItemRateOthers RoroHandlingRateOthers
    {
      get { return GetProperty(_RoroHandlingRateOthers); }
      set
      {
        SetProperty(_RoroHandlingRateOthers, value);
        this.ComputeStatementOfAccount();
      }
    }
    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_BillingDate));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_BillLadingNo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Consignee));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Vessel));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Cargo));
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
      LoadProperty(_BillingItemType, 2);
      LoadProperty(_UserAccount, 1);
      LoadProperty(_CreatedDate, DateTime.Now);
      LoadProperty(_LastUpdatedDate, DateTime.Now);
      //LoadProperty(_RoroHandlingRates, BillingItemRates.New());
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
	                              custpreferredaddress,vessel,voyageno,billingitem.item,itemcount,
                                itemcounthandling, item.handlingunit,preferreduom,billingitem.remarks,
                                grossamount, netamount
                              FROM billingitem 
                                LEFT JOIN item ON item.item = billingitem.item
                              WHERE billingitem = @id";
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
              LoadProperty(_Cargo, dr.GetInt32("item"));
              LoadProperty(_ItemCount, dr.GetDecimal("itemcount"));
              LoadProperty(_ItemCountHandling, dr.GetDecimal("itemcounthandling"));
              LoadProperty(_ItemUnit, dr.GetInt32("preferreduom"));
              LoadProperty(_Remarks, dr.GetString("remarks"));
              LoadProperty(_HandlingUnit, dr.GetInt32("handlingunit"));
              LoadProperty(_GrossAmount, dr.GetDecimal("grossamount"));
              LoadProperty(_NetAmount, dr.GetDecimal("netamount"));
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
          cmd.CommandText = @"INSERT INTO billingitem(type,referenceno,billingdate,billladingno,customer,custpreferredaddress,grossamount,
                                                         vessel,voyageno,item,itemcount,itemcounthandling,preferreduom,remarks,useraccount,createddate,lastupdateddate)
                                        VALUES (@type,@referenceno,@billingdate,@billladingno,@customer,@custpreferredaddress,@grossamount,
                                                         @vessel,@voyageno,@item,@itemcount,@itemcounthandling,@uom,@remarks,@useraccount,@createddate,@lastupdateddate)
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
          cmd.Parameters.AddWithValue("@item", Cargo);
          cmd.Parameters.AddWithValue("@itemcount", ItemCount);
          cmd.Parameters.AddWithValue("@uom", ItemUnit);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@useraccount", UserAccount);
          cmd.Parameters.AddWithValue("@createddate", CreatedDate.DBValue);
          cmd.Parameters.AddWithValue("@lastupdateddate", LastUpdatedDate.DBValue);
          if (GrossAmount != null)
            cmd.Parameters.AddWithValue("@grossamount", GrossAmount);
          else
            cmd.Parameters.AddWithValue("@grossamount", DBNull.Value);

          if (ItemCountHandling != null)
            cmd.Parameters.AddWithValue("@itemcounthandling", ItemCountHandling);
          else
            cmd.Parameters.AddWithValue("@itemcounthandling", DBNull.Value);

          //if (ItemReference != null)
          //  cmd.Parameters.AddWithValue("@itemreference", ItemReference);
          //else
          //  cmd.Parameters.AddWithValue("@itemreference", DBNull.Value);

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
                                            itemcounthandling = @itemcounthandling,
                                            preferreduom = @uom,
                                            remarks = @remarks,
                                            useraccount = @useraccount,
                                            lastupdateddate = @lastupdateddate,
                                            grossamount = @grossamount
                                        WHERE billingitem = @id";

          cmd.Parameters.AddWithValue("@referenceno", ReferenceNo);
          cmd.Parameters.AddWithValue("@billingdate", BillingDate.DBValue);
          cmd.Parameters.AddWithValue("@billladingno", BillLadingNo);
          cmd.Parameters.AddWithValue("@customer", Consignee);
          cmd.Parameters.AddWithValue("@custpreferredaddress", ConsigneeAddress);
          cmd.Parameters.AddWithValue("@vessel", Vessel);
          cmd.Parameters.AddWithValue("@voyageno", VoyageNo);
          cmd.Parameters.AddWithValue("@item", Cargo);
          cmd.Parameters.AddWithValue("@itemcount", ItemCount);
          cmd.Parameters.AddWithValue("@uom", ItemUnit);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@useraccount", 1);
          cmd.Parameters.AddWithValue("@lastupdateddate", DateTime.Now);
          cmd.Parameters.AddWithValue("@id", this.Id);
          if (GrossAmount != null)
            cmd.Parameters.AddWithValue("@grossamount", GrossAmount);
          else
            cmd.Parameters.AddWithValue("@grossamount", DBNull.Value);

          if (ItemCountHandling != null)
            cmd.Parameters.AddWithValue("@itemcounthandling", ItemCountHandling);
          else
            cmd.Parameters.AddWithValue("@itemcounthandling", DBNull.Value);

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
      this.SaveChild();
    }

    #endregion

    #region ChildFetch

    private void ChildFetch()
    {
      LoadProperty(_RoroHandlingRates, BillingItemRates.Get(new SingleCriteria<int>(this.Id)));
      LoadProperty(_RoroHandlingRateOthers, BillingItemRateOthers.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region SaveChild

    private void SaveChild()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_RoroHandlingRates), new SingleCriteria<int>(this.Id));
      Csla.DataPortal.UpdateChild(ReadProperty(_RoroHandlingRateOthers), new SingleCriteria<int>(this.Id));
    }

    #endregion

    public void ComputeStatementOfAccount()
    {
      this.GrossAmount = 0;
      if (this.RoroHandlingRates != null)
      {
        foreach (var rate in this.RoroHandlingRates)
        {
          var fixedamount = rate.Computation2 != null && rate.Computation2 > 0 ? rate.Computation2.Value : 1;
          this.GrossAmount += this.ItemCountHandling.Value * fixedamount * rate.Computation3;
        }
      }

      //  this.NetAmount = this.GrossAmount;
      //  if (this.NetAmount != null && this.NetAmount.Value > 0)
      //  {
      //    foreach (var other in this.RoroHandlingRateOthers.Where(o => o.RateType == 0))
      //    {
      //      var fixedamount = other.FixedAmount != null ? other.FixedAmount.Value : 0;
      //      this.NetAmount += fixedamount + (this.NetAmount * (other.Percentage.Value / (decimal)100.0));
      //    }
      //    var tempNetAmount = this.NetAmount;
      //    foreach (var other in this.RoroHandlingRateOthers.Where(o => o.RateType != 0))
      //    {
      //      var fixedamount = other.FixedAmount != null ? other.FixedAmount.Value : 0;
      //      this.NetAmount = (this.NetAmount.Value - fixedamount) - (tempNetAmount * (other.Percentage.Value / (decimal)100.0));
      //    }
      //  }
    }

    #endregion
  }

}
