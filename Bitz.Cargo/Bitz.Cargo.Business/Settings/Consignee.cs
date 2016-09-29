using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Settings
{
  [Serializable]
  public class Consignee : BusinessBase<Consignee>
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

    #region ContactType

    public static readonly PropertyInfo<int> _ContactType = RegisterProperty<int>(c => c.ContactType, "Type");
    public int ContactType
    {
      get { return GetProperty(_ContactType); }
      set { SetProperty(_ContactType, value); }
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

    #region PreferredName

    public static readonly PropertyInfo<string> _PreferredName = RegisterProperty<string>(c => c.PreferredName, "Preferred Name");
    public string PreferredName
    {
      get { return GetProperty(_PreferredName); }
      set { SetProperty(_PreferredName, value); }
    }
    #endregion

    #region Phone

    public static readonly PropertyInfo<string> _Phone = RegisterProperty<string>(c => c.Phone, "Phone");
    public string Phone
    {
      get { return GetProperty(_Phone); }
      set { SetProperty(_Phone, value); }
    }
    #endregion

    #region Fax

    public static readonly PropertyInfo<string> _Fax = RegisterProperty<string>(c => c.Fax, "Fax");
    public string Fax
    {
      get { return GetProperty(_Fax); }
      set { SetProperty(_Fax, value); }
    }
    #endregion

    #region Email

    public static readonly PropertyInfo<string> _Email = RegisterProperty<string>(c => c.Email, "Email");
    public string Email
    {
      get { return GetProperty(_Email); }
      set { SetProperty(_Email, value); }
    }
    #endregion

    #region Firstname

    public static readonly PropertyInfo<string> _Firstname = RegisterProperty<string>(c => c.Firstname, "Firstname");
    public string Firstname
    {
      get { return GetProperty(_Firstname); }
      set { SetProperty(_Firstname, value); }
    }
    #endregion

    #region MiddleName

    public static readonly PropertyInfo<string> _MiddleName = RegisterProperty<string>(c => c.MiddleName, "Middle Name");
    public string MiddleName
    {
      get { return GetProperty(_MiddleName); }
      set { SetProperty(_MiddleName, value); }
    }
    #endregion

    #region LastName

    public static readonly PropertyInfo<string> _LastName = RegisterProperty<string>(c => c.LastName, "Last Name");
    public string LastName
    {
      get { return GetProperty(_LastName); }
      set { SetProperty(_LastName, value); }
    }
    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_PreferredName));
    }

    #endregion

    #region Factory Methods

    public static void Get(int id, EventHandler<DataPortalResult<Consignee>> completed)
    {
      DataPortal<Consignee> dp = new DataPortal<Consignee>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(id);
    }

    public static void New(EventHandler<DataPortalResult<Consignee>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_ContactType, 2);
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
          cmd.CommandText = @"SELECT contact
                              ,code
                              ,name
                              ,phone
                              ,fax
                              ,email
                              ,firstname
                              ,middlename
                              ,lastname
                          FROM contact WHERE contact = @id";
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("contact"));
              LoadProperty(_Code, dr.GetString("code"));
              LoadProperty(_PreferredName, dr.GetString("name"));
              LoadProperty(_Phone, dr.GetString("phone"));
              LoadProperty(_Fax, dr.GetString("fax"));
              LoadProperty(_Email, dr.GetString("email"));
              LoadProperty(_Firstname, dr.GetString("firstname"));
              LoadProperty(_MiddleName, dr.GetString("middlename"));
              LoadProperty(_LastName, dr.GetString("lastname"));
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
          cmd.CommandText = @"INSERT INTO contact (code,name,contacttype,phone,fax,email,firstname,middlename,lastname)
                              VALUES (@code,@name,@contacttype,@phone,@fax,@email,@firstname,@middlename,@lastname)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", PreferredName);
          cmd.Parameters.AddWithValue("@contacttype", ContactType);
          cmd.Parameters.AddWithValue("@phone", Phone);
          cmd.Parameters.AddWithValue("@fax", Fax);
          cmd.Parameters.AddWithValue("@email", Email);
          cmd.Parameters.AddWithValue("@firstname", Firstname);
          cmd.Parameters.AddWithValue("@middlename", MiddleName);
          cmd.Parameters.AddWithValue("@lastname", LastName);

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
          cmd.CommandText = @"UPDATE contact
                               SET code = @code
                                  ,name = @name
                                  ,phone = @phone
                                  ,fax = @fax
                                  ,email = @email
                                  ,firstname = @firstname
                                  ,middlename = @middlename
                                  ,lastname = @lastname
                                WHERE contact = @id";

          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", PreferredName);
          cmd.Parameters.AddWithValue("@phone", Phone);
          cmd.Parameters.AddWithValue("@fax", Fax);
          cmd.Parameters.AddWithValue("@email", Email);
          cmd.Parameters.AddWithValue("@firstname", Firstname);
          cmd.Parameters.AddWithValue("@middlename", MiddleName);
          cmd.Parameters.AddWithValue("@lastname", LastName);
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
