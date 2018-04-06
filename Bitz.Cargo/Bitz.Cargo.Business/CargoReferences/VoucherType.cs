using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.CargoReferences
{
  [Serializable]
  public class VoucherType : BusinessBase<VoucherType>
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

    public static readonly PropertyInfo<int?> _Type = RegisterProperty<int?>(c => c.Type, "Type");
    public int? Type
    {
      get { return GetProperty(_Type); }
      set { SetProperty(_Type, value); }
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

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {      
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Type));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Name));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Name, 100));
  
    }

    #endregion
    
    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<VoucherType>> completed)
    {
      DataPortal<VoucherType> dp = new DataPortal<VoucherType>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<VoucherType>> completed)
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
          cmd.CommandText = @"SELECT vouchertype,type,name
                              FROM vouchertype WHERE vouchertype = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("VoucherType"));
              LoadProperty(_Type, dr.GetInt32("type"));
              LoadProperty(_Name, dr.GetString("name"));
            }
          }
        }
      }
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO vouchertype (type,name)
                              VALUES (@type,@name)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@type", Type);
          cmd.Parameters.AddWithValue("@name", Name);

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

    #region Update

    protected override void DataPortal_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE vouchertype SET
                                  type = @type,
                                  name = @name
                              WHERE vouchertype = @id";

          cmd.Parameters.AddWithValue("@type", Type);
          cmd.Parameters.AddWithValue("@name", Name);
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
  }
}
