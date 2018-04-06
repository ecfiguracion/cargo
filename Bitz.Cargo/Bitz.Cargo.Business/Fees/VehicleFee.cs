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
  public class VehicleFee : BusinessBase<VehicleFee>
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

    #region FeeNo

    public static readonly PropertyInfo<string> _FeeNo = RegisterProperty<string>(c => c.FeeNo, "Fee No.");
    public string FeeNo
    {
      get { return GetProperty(_FeeNo); }
      set { SetProperty(_FeeNo, value); }
    }

    #endregion

    #region Date

    public static readonly PropertyInfo<SmartDate> _Date = RegisterProperty<SmartDate>(c => c.Date, "Date");
    public SmartDate Date
    {
      get { return GetProperty(_Date); }
      set { SetProperty(_Date, value); }
    }

    #endregion

    #region PPATotal

    public static readonly PropertyInfo<decimal> _PPATotal = RegisterProperty<decimal>(c => c.PPATotal);
    public decimal PPATotal
    {
      get { return GetProperty(_PPATotal); }
      set { 
        SetProperty(_PPATotal, value);
      }
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

    #region VehicleFeeItems

    public static readonly PropertyInfo<VehicleFeeItems> _VehicleFeeItems = RegisterProperty<VehicleFeeItems>(c => c.VehicleFeeItems);
    public VehicleFeeItems VehicleFeeItems
    {
      get { return GetProperty(_VehicleFeeItems); }
      set { SetProperty(_VehicleFeeItems, value); }
    }

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Date));
    }

    #endregion

    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<VehicleFee>> completed)
    {
      DataPortal<VehicleFee> dp = new DataPortal<VehicleFee>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<VehicleFee>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_FeeNo, "[Auto-Number]");
      LoadProperty(_Date, DateTime.Now);
      LoadProperty(_Status, CargoConstants.StandardStatus.Draft.Id);
      LoadProperty(_VehicleFeeItems, VehicleFeeItems.New());
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
          cmd.CommandText = @"SELECT vehiclefee,feeno,date,status,ppatotal,createdby,datecreated,updatedby,dateupdated
                            FROM vehiclefee
                            WHERE vehiclefee = @id";
          cmd.Parameters.AddWithValue("@id", id);
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("vehiclefee"));
              LoadProperty(_FeeNo, dr.GetString("feeno"));
              LoadProperty(_Date, dr.GetSmartDate("date"));
              LoadProperty(_PPATotal, dr.GetDecimal("ppatotal"));
              LoadProperty(_Status, dr.GetInt32("status"));
              LoadProperty(_CreatedBy, dr.GetInt32("createdby"));
              LoadProperty(_DateCreated, dr.GetSmartDate("datecreated"));
              LoadProperty(_UpdatedBy, dr.GetInt32("updatedby"));
              LoadProperty(_DateUpdated, dr.GetSmartDate("dateupdated"));
            }
          }
        }
      }
      LoadProperty(_VehicleFeeItems, VehicleFeeItems.Get(new SingleCriteria<int>(this.Id)));
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO vehiclefee(date,feeno,ppatotal,status,createdby,datecreated,updatedby,dateupdated)
                              VALUES (@date,@feeno,@ppatotal,@status,@createdby,@datecreated,@updatedby,@dateupdated)
                              SELECT SCOPE_IDENTITY()";
          LoadProperty(_FeeNo, "RRTF"+DateTime.Now.ToString("yyMMdd-HHmmss"));
          cmd.Parameters.AddWithValue("@date", Date.DBValue);
          cmd.Parameters.AddWithValue("@feeno", FeeNo);
          cmd.Parameters.AddWithValue("@ppatotal", PPATotal);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_VehicleFeeItems), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE vehiclefee SET
                                  date = @date,
                                  feeno = @feeno,
                                  ppatotal = @ppatotal,
                                  status = @status,
                                  createdby = @createdby,
                                  datecreated = @datecreated,
                                  updatedby = @updatedby,
                                  dateupdated = @dateupdated
                              WHERE vehiclefee = @id";
          cmd.Parameters.AddWithValue("@date", Date.DBValue);
          cmd.Parameters.AddWithValue("@feeno", FeeNo);
          cmd.Parameters.AddWithValue("@ppatotal", PPATotal);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_VehicleFeeItems), new SingleCriteria<int>(this.Id));
    }

    #endregion


    #endregion

    #region Methods

    #endregion
  }

}
