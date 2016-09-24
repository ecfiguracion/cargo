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
  public class UnitOfMeasure : BusinessBase<UnitOfMeasure>
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

    #region Description

    public static readonly PropertyInfo<string> _Description = RegisterProperty<string>(c => c.Description, "Description");
    public string Description
    {
      get { return GetProperty(_Description); }
      set { SetProperty(_Description, value); }
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

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Name));
    }

    #endregion
    
    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<UnitOfMeasure>> completed)
    {
      DataPortal<UnitOfMeasure> dp = new DataPortal<UnitOfMeasure>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<UnitOfMeasure>> completed)
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
          cmd.CommandText = @"SELECT uom
                              ,code
                              ,name
                              ,description
                          FROM uom WHERE uom = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("uom"));
              LoadProperty(_Code, dr.GetString("code"));
              LoadProperty(_Name, dr.GetString("name"));
              LoadProperty(_Description, dr.GetString("description"));
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
          cmd.CommandText = @"INSERT INTO uom (code,name,description)
                              VALUES (@code,@name,@description)
                                        SELECT SCOPE_IDENTITY()";

          //if (ItemType != null)
          //  cmd.Parameters.AddWithValue("@itemtype", ItemType);
          //else
          //  cmd.Parameters.AddWithValue("@itemtype", DBNull.Value);

          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@description", Description);

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
          cmd.CommandText = @"UPDATE uom
                               SET code = @code
                                  ,name = @name
                                  ,description = @description
                                WHERE uom = @id";

          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@description", Description);
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
