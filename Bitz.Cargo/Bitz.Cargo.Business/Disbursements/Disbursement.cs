using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
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

namespace Bitz.Cargo.Business.Disbursements
{
  [Serializable]
  public class Disbursement : BusinessBase<Disbursement>
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

    #region Type

    public static readonly PropertyInfo<VoucherTypeInfo> _Type = RegisterProperty<VoucherTypeInfo>(c => c.Type, "Type");
    public VoucherTypeInfo Type
    {
      get { return GetProperty(_Type); }
      set { SetProperty(_Type, value); }
    }

    #endregion

    #region DocumentNo

    public static readonly PropertyInfo<string> _DocumentNo = RegisterProperty<string>(c => c.DocumentNo, "Document No.");
    public string DocumentNo
    {
      get { return GetProperty(_DocumentNo); }
      set { SetProperty(_DocumentNo, value); }
    }

    #endregion

    #region DocumentDate

    public static readonly PropertyInfo<SmartDate> _DocumentDate = RegisterProperty<SmartDate>(c => c.DocumentDate, "Document Date");
    public SmartDate DocumentDate
    {
      get { return GetProperty(_DocumentDate); }
      set { SetProperty(_DocumentDate, value); }
    }

    #endregion

    #region Recipient

    public static readonly PropertyInfo<string> _Recipient = RegisterProperty<string>(c => c.Recipient, "Recipient");
    public string Recipient
    {
      get { return GetProperty(_Recipient); }
      set { SetProperty(_Recipient, value); }
    }

    #endregion

    #region ControlNumber

    public static readonly PropertyInfo<int?> _ControlNumber = RegisterProperty<int?>(c => c.ControlNumber, "Control Number");
    public int? ControlNumber
    {
      get { return GetProperty(_ControlNumber); }
      set { SetProperty(_ControlNumber, value); }
    }

    #endregion

    #region PlateNo

    public static readonly PropertyInfo<string> _PlateNo = RegisterProperty<string>(c => c.PlateNo, "Plate No.");
    public string PlateNo
    {
      get { return GetProperty(_PlateNo); }
      set { SetProperty(_PlateNo, value); }
    }

    #endregion

    #region Vessel

    public static readonly PropertyInfo<VesselInfo> _Vessel = RegisterProperty<VesselInfo>(c => c.Vessel, "Vessel");
    public VesselInfo Vessel
    {
      get { return GetProperty(_Vessel); }
      set { SetProperty(_Vessel, value); }
    }

    #endregion

    #region PreparedBy

    public static readonly PropertyInfo<string> _PreparedBy = RegisterProperty<string>(c => c.PreparedBy, "Prepared By");
    public string PreparedBy
    {
      get { return GetProperty(_PreparedBy); }
      set { SetProperty(_PreparedBy, value); }
    }

    #endregion

    #region ApprovedBy

    public static readonly PropertyInfo<string> _ApprovedBy = RegisterProperty<string>(c => c.ApprovedBy, "Approved By");
    public string ApprovedBy
    {
      get { return GetProperty(_ApprovedBy); }
      set { SetProperty(_ApprovedBy, value); }
    }

    #endregion

    #region ReceivedBy

    public static readonly PropertyInfo<string> _ReceivedBy = RegisterProperty<string>(c => c.ReceivedBy, "Received By");
    public string ReceivedBy
    {
      get { return GetProperty(_ReceivedBy); }
      set { SetProperty(_ReceivedBy, value); }
    }

    #endregion

    #region Status

    public static readonly PropertyInfo<int> _Status = RegisterProperty<int>(c => c.Status);
    public CoreConstants.IdValue Status
    {
      get
      {
        var status = CargoConstants.StandardStatus.Items.SingleOrDefault(x => x.Id == GetProperty(_Status));
        if (status == null)
          return CargoConstants.StandardStatus.Draft;
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

    #region Derived Properties

    #endregion

    #region One To Many Properties

    #region DisbursementItems

    public static readonly PropertyInfo<DisbursementItems> _DisbursementItems = RegisterProperty<DisbursementItems>(c => c.DisbursementItems);
    public DisbursementItems DisbursementItems
    {
      get { return GetProperty(_DisbursementItems); }
      set { SetProperty(_DisbursementItems, value); }
    }

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_DocumentNo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_DocumentDate));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Recipient));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ControlNumber));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Type));
    }

    #endregion

    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Disbursement>> completed)
    {
      DataPortal<Disbursement> dp = new DataPortal<Disbursement>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Disbursement>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_DocumentNo, "[Auto-Number]");
      LoadProperty(_DocumentDate, DateTime.Now);
      LoadProperty(_Status, CargoConstants.StandardStatus.Draft.Id);
      LoadProperty(_CreatedBy, 1);
      LoadProperty(_DateCreated, DateTime.Now);
      LoadProperty(_DisbursementItems, DisbursementItems.New());
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
                                  SELECT d.disbursement,d.type,d.documentno,d.documentdate, d.recipient,d.controlnumber, d.plateno,
                                    d.preparedby, d.approvedby,d.receivedby,d.createdby,d.datecreated,d.updatedby,d.dateupdated,
	                                  d.status,v.contact AS {0}contact,v.code AS {0}code,v.name AS {0}name,
                                    vt.vouchertype AS {1}vouchertype,vt.type AS {1}type,vt.name AS {1}name
                                  FROM disbursement d
                                  LEFT JOIN contact v ON d.vessel = v.contact
                                  LEFT JOIN vouchertype vt ON d.type = vt.vouchertype
                                  WHERE disbursement = @id", _Vessel.Name,_Type.Name);
          cmd.Parameters.AddWithValue("@id", id);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("disbursement"));
              LoadProperty(_Type, dr.GetInt32("type"));
              LoadProperty(_DocumentNo, dr.GetString("documentno"));
              LoadProperty(_DocumentDate, dr.GetSmartDate("documentdate"));
              LoadProperty(_Recipient, dr.GetString("recipient"));
              LoadProperty(_ControlNumber, dr.GetInt32("controlnumber"));
              LoadProperty(_PlateNo, dr.GetString("plateno"));
              LoadProperty(_PreparedBy, dr.GetString("preparedby"));
              LoadProperty(_ApprovedBy, dr.GetString("approvedby"));
              LoadProperty(_ReceivedBy, dr.GetString("receivedby"));
              LoadProperty(_Status, dr.GetInt32("status"));
              LoadProperty(_CreatedBy, dr.GetInt32("createdby"));
              LoadProperty(_DateCreated, dr.GetSmartDate("datecreated"));
              LoadProperty(_UpdatedBy, dr.GetInt32("updatedby"));
              LoadProperty(_DateUpdated, dr.GetSmartDate("dateupdated"));
              LoadProperty(_Vessel, BaseContactInfo.Get(dr, _Vessel.Name));
              LoadProperty(_Type, VoucherTypeInfo.Get(dr, _Type.Name));
            }
          }
        }
      }
      LoadProperty(_DisbursementItems, DisbursementItems.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO disbursement(type,documentno,documentdate,recipient,controlnumber,plateno,preparedby,
                                      approvedby,receivedby, vessel,status,createdby,datecreated,updatedby,dateupdated)
                              VALUES (@type,@documentno,@documentdate,@plateno,@preparedby,@approvedby,@receivedby,
                                      @vessel,@status,@createdby,@datecreated,@updatedby,@dateupdated)
                              SELECT SCOPE_IDENTITY()";
          LoadProperty(_DocumentNo, "DISB"+DateTime.Now.ToString("yyMMdd-HHmmss"));
          cmd.Parameters.AddWithValue("@documentno", DocumentNo);
          cmd.Parameters.AddWithValue("@documentdate", DocumentDate.DBValue);
          cmd.Parameters.AddWithValue("@recipient", Recipient);
          cmd.Parameters.AddWithValue("@controlnumber", ControlNumber);
          cmd.Parameters.AddWithValue("@plateno", PlateNo);
          cmd.Parameters.AddWithValue("@preparedby", PreparedBy);
          cmd.Parameters.AddWithValue("@approvedby", ApprovedBy);
          cmd.Parameters.AddWithValue("@receivedby", ReceivedBy);
          if (Vessel != null)
            cmd.Parameters.AddWithValue("@vessel", Vessel.Id);
          else
            cmd.Parameters.AddWithValue("@vessel", DBNull.Value);
          cmd.Parameters.AddWithValue("@type", Type.Id);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_DisbursementItems), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE disbursement SET
                                  documentdate = @documentdate,
                                  recipient = @recipient,
                                  controlnumber = @controlnumber,
                                  plateno = @plateno,
                                  preparedby = @preparedby,
                                  approvedby = @approvedby,
                                  receivedby = @receivedby,
                                  vessel = @vessel,
                                  status = @status,
                                  createdby = @createdby,
                                  datecreated = @datecreated,
                                  updatedby = @updatedby,
                                  dateupdated = @dateupdated
                              WHERE Disbursement = @id";
          cmd.Parameters.AddWithValue("@documentdate", DocumentDate.DBValue);
          cmd.Parameters.AddWithValue("@recipient", Recipient);
          cmd.Parameters.AddWithValue("@controlnumber", ControlNumber);
          cmd.Parameters.AddWithValue("@plateno", PlateNo);
          cmd.Parameters.AddWithValue("@preparedby", PreparedBy);
          cmd.Parameters.AddWithValue("@approvedby", ApprovedBy);
          cmd.Parameters.AddWithValue("@receivedby", ReceivedBy);

          if (Vessel != null)
            cmd.Parameters.AddWithValue("@vessel", Vessel.Id);
          else
            cmd.Parameters.AddWithValue("@vessel", DBNull.Value);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_DisbursementItems), new SingleCriteria<int>(this.Id));
    }

    #endregion


    #endregion

    #region Methods

    #endregion
  }

}
