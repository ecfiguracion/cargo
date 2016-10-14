using Bitz.Business.Contacts.Infos;
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
  public class Payment : BusinessBase<Payment>
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

    #region PaymentNo

    public static readonly PropertyInfo<string> _PaymentNo = RegisterProperty<string>(c => c.PaymentNo, "Reference No.");
    public string PaymentNo
    {
      get { return GetProperty(_PaymentNo); }
      set { SetProperty(_PaymentNo, value); }
    }

    #endregion

    #region PaymentDate

    public static readonly PropertyInfo<SmartDate> _PaymentDate = RegisterProperty<SmartDate>(c => c.PaymentDate, "Billing Date");
    public SmartDate PaymentDate
    {
      get { return GetProperty(_PaymentDate); }
      set { SetProperty(_PaymentDate, value); }
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
      }
    }

    #endregion

    #region ORNumber

    public static readonly PropertyInfo<string> _ORNumber = RegisterProperty<string>(c => c.ORNumber, "ORNumber");
    public string ORNumber
    {
      get { return GetProperty(_ORNumber); }
      set { SetProperty(_ORNumber, value); }
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

    #endregion

    #region One To Many Properties

    #region PaymentDetails

    public static readonly PropertyInfo<PaymentDetails> _PaymentDetails = RegisterProperty<PaymentDetails>(c => c.PaymentDetails);
    public PaymentDetails PaymentDetails
    {
      get { return GetProperty(_PaymentDetails); }
      set { SetProperty(_PaymentDetails, value); }
    }

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_PaymentDate));
      BusinessRules.AddRule(new ConsigneeRequired { PrimaryProperty = _Consignee });
    }

    #region ConsigneeRequired
    private class ConsigneeRequired : Csla.Rules.BusinessRule
    {
      protected override void Execute(Csla.Rules.RuleContext context)
      {
        var target = (Payment)context.Target;
        if (target.Consignee == null)
        {
          context.AddErrorResult("Consignee is required.");
        }
      }
    }
    #endregion

    #endregion

    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Payment>> completed)
    {
      DataPortal<Payment> dp = new DataPortal<Payment>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Payment>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_PaymentNo, "[Auto-Number]");
      LoadProperty(_PaymentDate, DateTime.Now);
      LoadProperty(_CreatedBy, 1);
      LoadProperty(_DateCreated, DateTime.Now);
      LoadProperty(_Status, CargoConstants.PaymentStatus.Draft.Id);
      LoadProperty(_PaymentDetails, PaymentDetails.New());
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
                            SELECT p.payment,p.paymentno,p.paymentdate,p.ornumber,p.remarks,p.status,
                              p.createdby,p.datecreated,p.updatedby,p.dateupdated,
	                            c.contact AS {0}contact,c.code AS {0}code,c.name AS {0}name
                            FROM payment p
                            LEFT JOIN contact c ON p.consignee = c.contact
                            WHERE p.payment = @id", _Consignee.Name);            
          cmd.Parameters.AddWithValue("@id", id);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("payment"));
              LoadProperty(_PaymentNo, dr.GetString("paymentno"));
              LoadProperty(_PaymentDate, dr.GetSmartDate("paymentdate"));
              LoadProperty(_Consignee, BaseContactInfo.Get(dr, _Consignee.Name));
              LoadProperty(_ORNumber, dr.GetString("ornumber"));
              LoadProperty(_Remarks, dr.GetString("remarks"));
              LoadProperty(_Status, dr.GetInt32("status"));
              LoadProperty(_CreatedBy, dr.GetInt32("createdby"));
              LoadProperty(_DateCreated, dr.GetSmartDate("datecreated"));
              LoadProperty(_UpdatedBy, dr.GetInt32("updatedby"));
              LoadProperty(_DateUpdated, dr.GetSmartDate("dateupdated"));
            }
          }
        }
      }
      LoadProperty(_PaymentDetails, PaymentDetails.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO payment(paymentno,paymentdate,consignee,ornumber,remarks,status,createdby,datecreated,updatedby,dateupdated)
                              VALUES (@paymentno,@paymentdate,@consignee,@ornumber,@remarks,@status,@createdby,@datecreated,@updatedby,@dateupdated)
                              SELECT SCOPE_IDENTITY()";
          LoadProperty(_PaymentNo, "PYM" + DateTime.Now.ToString("yyMMdd-HHmmss"));
          cmd.Parameters.AddWithValue("@paymentno", PaymentNo);
          cmd.Parameters.AddWithValue("@paymentdate", PaymentDate.DBValue);
          cmd.Parameters.AddWithValue("@consignee", Consignee.Id);
          cmd.Parameters.AddWithValue("@ornumber", ORNumber);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@status", Status.Id);
          cmd.Parameters.AddWithValue("@createdby", CreatedBy);
          cmd.Parameters.AddWithValue("@datecreated", DateCreated.DBValue);
          cmd.Parameters.AddWithValue("@updatedby", UpdatedBy);
          cmd.Parameters.AddWithValue("@dateupdated", DateUpdated.DBValue);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_PaymentDetails), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE payment SET
                                     paymentdate = @paymentdate,
                                     consignee = @consignee,
                                     ornumber = @ornumber,
                                     remarks = @remarks,
                                     status = @status,
                                     createdby = @createdby,
                                     datecreated = @datecreated,
                                     updatedby = @updatedby,
                                     dateupdated = @dateupdated
                              WHERE payment = @id";
          cmd.Parameters.AddWithValue("@paymentdate", PaymentDate.DBValue);
          cmd.Parameters.AddWithValue("@consignee", Consignee.Id);
          cmd.Parameters.AddWithValue("@ornumber", ORNumber);
          cmd.Parameters.AddWithValue("@remarks", Remarks);
          cmd.Parameters.AddWithValue("@status", Status.Id);
          cmd.Parameters.AddWithValue("@createdby", CreatedBy);
          cmd.Parameters.AddWithValue("@datecreated", DateCreated.DBValue);
          cmd.Parameters.AddWithValue("@updatedby", UpdatedBy);
          cmd.Parameters.AddWithValue("@dateupdated", DateUpdated.DBValue);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_PaymentDetails), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #endregion
  }
}
