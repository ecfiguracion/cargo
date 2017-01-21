using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.Constants;
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

namespace Bitz.Cargo.Business.Billing
{
  [Serializable]
  public class Foreign : BusinessBase<Foreign>
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

    #region BillType

    public static readonly PropertyInfo<int> _BillType = RegisterProperty<int>(c => c.BillType, "Type");
    public int BillType
    {
      get { return GetProperty(_BillType); }
      set { SetProperty(_BillType, value); }
    }

    #endregion

    #region BillNo

    public static readonly PropertyInfo<string> _BillNo = RegisterProperty<string>(c => c.BillNo, "Reference No.");
    public string BillNo
    {
      get { return GetProperty(_BillNo); }
      set { SetProperty(_BillNo, value); }
    }

    #endregion

    #region BillDate

    public static readonly PropertyInfo<SmartDate> _BillDate = RegisterProperty<SmartDate>(c => c.BillDate, "Billing Date");
    public SmartDate BillDate
    {
      get { return GetProperty(_BillDate); }
      set
      {
        SetProperty(_BillDate, value);
        DueDate = new SmartDate(new DateTime(value.Date.Year, value.Date.Month, DateTime.DaysInMonth(value.Date.Year, value.Date.Month)));
      }
    }

    #endregion

    #region Consignee

    public static readonly PropertyInfo<BaseContactInfo> _Consignee = RegisterProperty<BaseContactInfo>(c => c.Consignee);
    public BaseContactInfo Consignee
    {
      get { return GetProperty(_Consignee); }
      set 
      { 
        SetProperty(_Consignee, value);
        this.GetWTaxRate();
      }
    }

    #endregion

    #region BillingAddress

    public static readonly PropertyInfo<string> _BillingAddress = RegisterProperty<string>(c => c.BillingAddress, "Consignee Address");
    public string BillingAddress
    {
      get { return GetProperty(_BillingAddress); }
      set { SetProperty(_BillingAddress, value); }
    }

    #endregion

    #region Vessel

    public static readonly PropertyInfo<BaseContactInfo> _Vessel = RegisterProperty<BaseContactInfo>(c => c.Vessel, "Vessel");
    public BaseContactInfo Vessel
    {
      get { return GetProperty(_Vessel); }
      set { SetProperty(_Vessel, value); }
    }

    #endregion

    #region BillOfLadingNo

    public static readonly PropertyInfo<string> _BillOfLadingNo = RegisterProperty<string>(c => c.BillOfLadingNo, "Bill Lading No.");
    public string BillOfLadingNo
    {
      get { return GetProperty(_BillOfLadingNo); }
      set { SetProperty(_BillOfLadingNo, value); }
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

    #region WTaxRate

    public static readonly PropertyInfo<decimal> _WTaxRate = RegisterProperty<decimal>(c => c.WTaxRate);
    public decimal WTaxRate
    {
      get { return GetProperty(_WTaxRate); }
      set { SetProperty(_WTaxRate, value); }
    }

    #endregion

    #region TotalBill

    public static readonly PropertyInfo<decimal> _TotalBill = RegisterProperty<decimal>(c => c.TotalBill);
    public decimal TotalBill
    {
      get { return GetProperty(_TotalBill); }
      set { SetProperty(_TotalBill, value); }
    }

    #endregion

    #region DueDate

    public static readonly PropertyInfo<SmartDate> _DueDate = RegisterProperty<SmartDate>(c => c.DueDate);
    public SmartDate DueDate
    {
      get { return GetProperty(_DueDate); }
      set { SetProperty(_DueDate, value); }
    }

    #endregion

    #region Status

    public static readonly PropertyInfo<int> _Status = RegisterProperty<int>(c => c.Status);
    public CoreConstants.IdValue Status
    {
      get 
      {
        var status = CargoConstants.BillStatus.Items.SingleOrDefault(x => x.Id == GetProperty(_Status));
        if (status == null)
          return CargoConstants.BillStatus.Draft;
        return status;
      }
      set 
      {
        SetProperty(_Status, value.Id); 
      }
    }

    #endregion

    #region CreatedBy

    public static readonly PropertyInfo<int> _CreatedBy = RegisterProperty<int>(c => c.CreatedBy);
    public int CreatedBy
    {
      get { return GetProperty(_CreatedBy); }
      set { SetProperty(_CreatedBy, value); }
    }



    #endregion

    #region DateCreated

    public static readonly PropertyInfo<SmartDate> _DateCreated = RegisterProperty<SmartDate>(c => c.DateCreated);
    public SmartDate DateCreated
    {
      get { return GetProperty(_DateCreated); }
      set { SetProperty(_DateCreated, value); }
    }

    #endregion

    #region UpdatedBy

    public static readonly PropertyInfo<int> _UpdatedBy = RegisterProperty<int>(c => c.UpdatedBy);
    public int UpdatedBy
    {
      get { return GetProperty(_UpdatedBy); }
      set { SetProperty(_UpdatedBy, value); }
    }
    #endregion

    #region DateUpdated

    public static readonly PropertyInfo<SmartDate> _DateUpdated = RegisterProperty<SmartDate>(c => c.DateUpdated);
    public SmartDate DateUpdated
    {
      get { return GetProperty(_DateUpdated); }
      set { SetProperty(_DateUpdated, value); }
    }

    #endregion

    #region IsIncludePayeeBankAccount

    public static readonly PropertyInfo<bool> _IsIncludePayeeBankAccount = RegisterProperty<bool>(c => c.IsIncludePayeeBankAccount);
    public bool IsIncludePayeeBankAccount
    {
      get { return GetProperty(_IsIncludePayeeBankAccount); }
      set { SetProperty(_IsIncludePayeeBankAccount, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #region BillItems

    public static readonly PropertyInfo<BillItems> _BillItems = RegisterProperty<BillItems>(c => c.BillItems);
    public BillItems BillItems
    {
      get { return GetProperty(_BillItems); }
      set { SetProperty(_BillItems, value); }
    }

    #endregion

    #region Payments

    public static readonly PropertyInfo<PaymentInfos> _Payments = RegisterProperty<PaymentInfos>(c => c.Payments);
    public PaymentInfos Payments
    {
      get { return GetProperty(_Payments); }
      set { SetProperty(_Payments, value); }
    }

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_BillDate));
      BusinessRules.AddRule(new ConsigneeRequired { PrimaryProperty = _Consignee });
      BusinessRules.AddRule(new VesselRequired { PrimaryProperty = _Vessel });
    }

    #region ConsigneeRequired
    private class ConsigneeRequired : Csla.Rules.BusinessRule
    {
      protected override void Execute(Csla.Rules.RuleContext context)
      {
        var target = (Foreign)context.Target;
        if (target.Consignee == null)
        {
          context.AddErrorResult("Consignee is required.");
        }
      }
    }
    #endregion

    #region VesselRequired
    private class VesselRequired : Csla.Rules.BusinessRule
    {
      protected override void Execute(Csla.Rules.RuleContext context)
      {
        var target = (Foreign)context.Target;
        if (target.Vessel == null)
        {
          context.AddErrorResult("Vessel is required.");
        }
      }
    }
    #endregion

    #endregion

    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Foreign>> completed)
    {
      DataPortal<Foreign> dp = new DataPortal<Foreign>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Foreign>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_BillNo, "[Auto-Number]");
      LoadProperty(_BillDate, DateTime.Now);
      LoadProperty(_DueDate,new SmartDate(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month))));
      LoadProperty(_BillType, CargoConstants.BillingType.Foreign.Id);
      LoadProperty(_CreatedBy, 1);
      LoadProperty(_DateCreated, DateTime.Now);
      LoadProperty(_Status, CargoConstants.BillStatus.Draft.Id);
      LoadProperty(_IsIncludePayeeBankAccount, false);
      LoadProperty(_BillItems, BillItems.New());
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
          cmd.CommandText = string.Format(@"
                            SELECT b.bill,b.billtype,b.billno,b.billdate,b.consignee,b.billingaddress,
	                            c.contact AS {0}contact,c.code AS {0}code,c.name AS {0}name,
	                            v.contact AS {1}contact,v.code AS {1}code,v.name AS {1}name,
	                            b.billofladingno,b.voyageno,b.wtaxrate,b.totalbill,b.duedate,b.status,
	                            billofladingno,b.createdby,b.datecreated,b.updatedby,b.dateupdated,b.isincludebank
                            FROM bill b
                            LEFT JOIN contact c ON b.consignee = c.contact
                            LEFT JOIN contactaddress cca ON c.contact = cca.contact
                            LEFT JOIN contact v ON b.vessel = v.contact
                            LEFT JOIN contactaddress vca ON v.contact = vca.contact
                            WHERE b.bill = @id", _Consignee.Name,_Vessel.Name);            
          cmd.Parameters.AddWithValue("@id", id);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("bill"));
              LoadProperty(_BillType, dr.GetInt32("billtype"));
              LoadProperty(_BillNo, dr.GetString("billno"));
              LoadProperty(_BillDate, dr.GetSmartDate("billdate"));
              LoadProperty(_Consignee, BaseContactInfo.Get(dr, _Consignee.Name));
              LoadProperty(_BillingAddress, dr.GetString("billingaddress"));
              LoadProperty(_Vessel, BaseContactInfo.Get(dr, _Vessel.Name));
              LoadProperty(_BillOfLadingNo, dr.GetString("BillOfLadingNo"));
              LoadProperty(_VoyageNo, dr.GetString("voyageno"));
              LoadProperty(_WTaxRate, dr.GetDecimal("wtaxrate"));
              LoadProperty(_TotalBill, dr.GetDecimal("totalbill"));
              LoadProperty(_DueDate, dr.GetSmartDate("duedate"));
              LoadProperty(_Status,dr.GetInt32("status"));
              LoadProperty(_CreatedBy, dr.GetInt32("createdby"));
              LoadProperty(_DateCreated, dr.GetSmartDate("datecreated"));
              LoadProperty(_UpdatedBy, dr.GetInt32("updatedby"));
              LoadProperty(_DateUpdated, dr.GetSmartDate("dateupdated"));
              LoadProperty(_IsIncludePayeeBankAccount, dr.GetBoolean("isincludebank"));
            }
          }
        }
      }
      LoadProperty(_BillItems, BillItems.Get(new SingleCriteria<int>(this.Id)));
      LoadProperty(_Payments, PaymentInfos.Get(new PaymentInfos.Criteria() { BillId = this.Id, Status = CargoConstants.PaymentStatus.Approved.Id }));
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO bill(billtype,billno,billdate,consignee,billingaddress,vessel,billofladingno,voyageno,
                                               wtaxrate,totalbill,duedate,status,createdby,datecreated,updatedby,dateupdated,isincludebank)
                              VALUES (@billtype,@billno,@billdate,@consignee,@billingaddress,@vessel,@billofladingno,@voyageno,
                                               @wtaxrate,@totalbill,@duedate,@status,@createdby,@datecreated,@updatedby,@dateupdated,@isincludebank)
                              SELECT SCOPE_IDENTITY()";
          LoadProperty(_BillNo, "FOR" + DateTime.Now.ToString("yyMMdd-HHmmss"));
          cmd.Parameters.AddWithValue("@billtype", BillType);
          cmd.Parameters.AddWithValue("@billno", BillNo);
          cmd.Parameters.AddWithValue("@billdate", BillDate.DBValue);
          cmd.Parameters.AddWithValue("@consignee", Consignee.Id);
          cmd.Parameters.AddWithValue("@billingaddress", BillingAddress);
          cmd.Parameters.AddWithValue("@vessel", Vessel.Id);
          cmd.Parameters.AddWithValue("@billofladingno", BillOfLadingNo);
          cmd.Parameters.AddWithValue("@voyageno", VoyageNo);
          cmd.Parameters.AddWithValue("@wtaxrate", WTaxRate);
          cmd.Parameters.AddWithValue("@totalbill", TotalBill);
          cmd.Parameters.AddWithValue("@duedate", DueDate.DBValue);
          cmd.Parameters.AddWithValue("@status", Status.Id);
          cmd.Parameters.AddWithValue("@createdby", CreatedBy);
          cmd.Parameters.AddWithValue("@datecreated", DateCreated.DBValue);
          cmd.Parameters.AddWithValue("@updatedby", UpdatedBy);
          cmd.Parameters.AddWithValue("@dateupdated", DateUpdated.DBValue);
          cmd.Parameters.AddWithValue("@isincludebank", IsIncludePayeeBankAccount);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_BillItems), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE bill SET
                                     billdate = @billdate,
                                     consignee = @consignee,
                                     billingaddress = @billingaddress,
                                     vessel = @vessel,
                                     billofladingno = @billofladingno,
                                     voyageno = @voyageno,
                                     wtaxrate = @wtaxrate,
                                     totalbill = @totalbill,
                                     duedate = @duedate,
                                     status = @status,
                                     updatedby = @updatedby,
                                     dateupdated = @dateupdated,
                                     isincludebank = @isincludebank
                              WHERE bill = @id";
          cmd.Parameters.AddWithValue("@billdate", BillDate.DBValue);
          cmd.Parameters.AddWithValue("@consignee", Consignee.Id);
          cmd.Parameters.AddWithValue("@billingaddress", BillingAddress);
          cmd.Parameters.AddWithValue("@vessel", Vessel.Id);
          cmd.Parameters.AddWithValue("@billofladingno", BillOfLadingNo);
          cmd.Parameters.AddWithValue("@voyageno", VoyageNo);
          cmd.Parameters.AddWithValue("@wtaxrate", WTaxRate);
          cmd.Parameters.AddWithValue("@totalbill", TotalBill);
          cmd.Parameters.AddWithValue("@duedate", DueDate.DBValue);
          cmd.Parameters.AddWithValue("@status", Status.Id);
          cmd.Parameters.AddWithValue("@updatedby", UpdatedBy);
          cmd.Parameters.AddWithValue("@dateupdated", DateUpdated.DBValue);
          cmd.Parameters.AddWithValue("@isincludebank", IsIncludePayeeBankAccount);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_BillItems), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #endregion

    #region Methods

    private void GetWTaxRate()
    {
      if (this.Consignee == null) return;

      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT wtaxrate
                              FROM consignee
                              WHERE contact = @id";
          cmd.Parameters.AddWithValue("@id", this.Consignee.Id);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              this.WTaxRate = dr.GetDecimal("wtaxrate");
            }
          }
        }
      }
    }

    #endregion
  }

}
