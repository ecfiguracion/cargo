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
  public class Signatory : BusinessBase<Signatory>
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

    #region Position

    public static readonly PropertyInfo<string> _Position = RegisterProperty<string>(c => c.Position, "Position");
    public string Position
    {
      get { return GetProperty(_Position); }
      set { SetProperty(_Position, value); }
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
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Name, 300));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Position, 300));
  
    }

    #endregion
    
    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Signatory>> completed)
    {
      DataPortal<Signatory> dp = new DataPortal<Signatory>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Signatory>> completed)
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
          cmd.CommandText = @"SELECT signatory,type,name,position
                              FROM signatory WHERE signatory = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("signatory"));
              LoadProperty(_Type, dr.GetInt32("type"));
              LoadProperty(_Name, dr.GetString("name"));
              LoadProperty(_Position, dr.GetString("position"));
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
          cmd.CommandText = @"INSERT INTO signatory (type,name,position)
                              VALUES (@type,@name,@position)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@type", Type.Value);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@position", Position);

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
          cmd.CommandText = @"UPDATE signatory SET
                                  type = @type,
                                  name = @name,
                                  position = @position
                              WHERE signatory = @id";

          cmd.Parameters.AddWithValue("@type", Type.Value);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@position", Position);
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
