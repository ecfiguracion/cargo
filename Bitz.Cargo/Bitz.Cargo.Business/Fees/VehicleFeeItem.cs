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
  #region VehicleFeeItem
  [Serializable]
  public class VehicleFeeItem : BusinessBase<VehicleFeeItem>
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

    #region VehicleFee

    public static readonly PropertyInfo<int> _VehicleFee = RegisterProperty<int>(c => c.VehicleFee, "VehicleFee");
    public int VehicleFee
    {
      get { return GetProperty(_VehicleFee); }
      set { SetProperty(_VehicleFee, value); }
    }

    #endregion

    #region InvoiceNo

    public static readonly PropertyInfo<string> _InvoiceNo = RegisterProperty<string>(c => c.InvoiceNo, "Invoice No");
    public string InvoiceNo
    {
      get { return GetProperty(_InvoiceNo); }
      set { SetProperty(_InvoiceNo, value); }
    }

    #endregion

    #region Vehicle

    public static readonly PropertyInfo<string> _Vehicle = RegisterProperty<string>(c => c.Vehicle, "Vehicle");
    public string Vehicle
    {
      get { return GetProperty(_Vehicle); }
      set { SetProperty(_Vehicle, value); }
    }

    #endregion

    #region PlateNo

    public static readonly PropertyInfo<string> _PlateNo = RegisterProperty<string>(c => c.PlateNo, "Plate No");
    public string PlateNo
    {
      get { return GetProperty(_PlateNo); }
      set { SetProperty(_PlateNo, value); }
    }

    #endregion

    #region VehicleType

    public static readonly PropertyInfo<int?> _VehicleType = RegisterProperty<int?>(c => c.VehicleType);
    public int? VehicleType
    {
      get { return GetProperty(_VehicleType); }
      set { SetProperty(_VehicleType, value); }
    }

    #endregion

    #region Fee

    public static readonly PropertyInfo<decimal?> _Fee = RegisterProperty<decimal?>(c => c.Fee);
    public decimal? Fee
    {
      get { return GetProperty(_Fee); }
      set { SetProperty(_Fee, value); }
    }

    #endregion

    #endregion

    #region Derived Properties

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_InvoiceNo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Vehicle));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_PlateNo));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_VehicleType));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_VehicleFee));
    }

    #endregion

    #region Factory Methods

    public static VehicleFeeItem New()
    {
      return Csla.DataPortal.CreateChild<VehicleFeeItem>();
    }

    public static VehicleFeeItem Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<VehicleFeeItem>(dr);
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
      LoadProperty(_Id, dr.GetInt32("vehiclefeeitem"));      
      LoadProperty(_VehicleFee, dr.GetInt32("vehiclefee"));
      LoadProperty(_InvoiceNo, dr.GetString("invoiceno"));
      LoadProperty(_Vehicle, dr.GetString("vehicle"));
      LoadProperty(_PlateNo, dr.GetString("plateno"));
      LoadProperty(_VehicleType, dr.GetInt32("vehicletype"));
      LoadProperty(_Fee, dr.GetDecimal("fee"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO vehiclefeeitem(vehiclefee,invoiceno,vehicle,plateno,vehicletype,fee)
                              VALUES (@vehiclefee,@invoiceno,@vehicle,@plateno,@vehicletype,@fee)
                            SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@vehiclefee", parentId.Value);
          cmd.Parameters.AddWithValue("@invoiceno", InvoiceNo);
          cmd.Parameters.AddWithValue("@vehicle", Vehicle);
          cmd.Parameters.AddWithValue("@plateno", PlateNo);
          cmd.Parameters.AddWithValue("@vehicletype", VehicleType);
          cmd.Parameters.AddWithValue("@fee", Fee);

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
          cmd.CommandText = @"UPDATE vehiclefeeitem SET 
                                vehiclefee = @vehiclefee,
                                invoiceno = @invoiceno,
                                vehicle = @vehicle,
                                plateno = @plateno,
                                vehicletype = @vehicletype,
                                fee = @fee                          
                              WHERE vehiclefeeitem = @id";
          cmd.Parameters.AddWithValue("@vehiclefee", parentId.Value);
          cmd.Parameters.AddWithValue("@invoiceno", InvoiceNo);
          cmd.Parameters.AddWithValue("@vehicle", Vehicle);
          cmd.Parameters.AddWithValue("@plateno", PlateNo);
          cmd.Parameters.AddWithValue("@vehicletype", VehicleType);
          cmd.Parameters.AddWithValue("@fee", Fee);
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
          cmd.CommandText = @"DELETE FROM vehiclefeeitem WHERE vehiclefeeitem = @id";
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

    #endregion
  }
  #endregion

  #region VehicleFeeItems
  [Serializable]
  public class VehicleFeeItems : BusinessListBase<VehicleFeeItems, VehicleFeeItem>
  {
    #region Factory Methods

    public static VehicleFeeItems New()
    {
      return new VehicleFeeItems();
    }

    public static VehicleFeeItems Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<VehicleFeeItems>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT vehiclefeeitem,vehiclefee,invoiceno,vehicle,plateno,vehicletype,fee  
                              FROM vehiclefeeitem
                              WHERE vehiclefee = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(VehicleFeeItem.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
