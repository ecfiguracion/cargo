using Bitz.Business.Settings.Infos;
using Bitz.Core.Constants;
using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Business.Contacts
{
  [Serializable]
  public class Contact : BusinessBase<Contact>
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

    public static readonly PropertyInfo<string> _Code = RegisterProperty<string>(c => c.Code, "Contact Code");
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

    #region FirstName

    public static readonly PropertyInfo<string> _FirstName = RegisterProperty<string>(c => c.FirstName, "FirstName");
    public string FirstName
    {
      get { return GetProperty(_FirstName); }
      set
      {
        SetProperty(_FirstName, value);
        this.Name = String.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
      }
    }

    #endregion

    #region LastName

    public static readonly PropertyInfo<string> _LastName = RegisterProperty<string>(c => c.LastName, "LastName");
    public string LastName
    {
      get { return GetProperty(_LastName); }
      set
      {
        SetProperty(_LastName, value);
        this.Name = String.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
      }
    }

    #endregion

    #region MiddleName

    public static readonly PropertyInfo<string> _MiddleName = RegisterProperty<string>(c => c.MiddleName, "MiddleName");
    public string MiddleName
    {
      get { return GetProperty(_MiddleName); }
      set
      {
        SetProperty(_MiddleName, value);
        this.Name = String.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
      }
    }

    #endregion

    #region ContactType

    public static readonly PropertyInfo<int> _ContactType = RegisterProperty<int>(c => c.ContactType);
    public int ContactType
    {
      get { return GetProperty(_ContactType); }
      set { SetProperty(_ContactType, value); }
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

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Code));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_ContactType));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Code, 30));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_Name));
      BusinessRules.AddRule(new FirstNameRule() { PrimaryProperty = _FirstName });
      BusinessRules.AddRule(new MiddleNameRule() { PrimaryProperty = _MiddleName });
      BusinessRules.AddRule(new LastNameRule() { PrimaryProperty = _LastName });
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Name, 100));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_FirstName, 20));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_MiddleName, 20));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_LastName, 30));
    }

    #region Custom Rules

    private class FirstNameRule : Csla.Rules.BusinessRule
    {
      protected override void Execute(RuleContext context)
      {
        var target = (Contact)context.Target;
        if (target.ContactType == BitzConstants.ContactTypes.Employee.Id &&
            target.FirstName.Length == 0)
        {
          context.AddErrorResult("Firstname required");
        }

      }
    }

    private class MiddleNameRule : Csla.Rules.BusinessRule
    {
      protected override void Execute(RuleContext context)
      {
        var target = (Contact)context.Target;
        if (target.ContactType == BitzConstants.ContactTypes.Employee.Id &&
            target.MiddleName.Length == 0)
        {
          context.AddErrorResult("Middle name required");

        }

      }
    }

    private class LastNameRule : Csla.Rules.BusinessRule
    {
      protected override void Execute(RuleContext context)
      {
        var target = (Contact)context.Target;
        if (target.ContactType == BitzConstants.ContactTypes.Employee.Id &&
            target.LastName.Length == 0)
        {
          context.AddErrorResult("Lastname required");

        }

      }
    }

    #endregion

    #endregion

    #region Factory Methods

    public static Contact Get(SafeDataReader dr, string columnalias)
    {
      return Csla.DataPortal.FetchChild<Contact>(dr, columnalias);
    }

    public static Contact New()
    {
      return Csla.DataPortal.CreateChild<Contact>();
    }

    public static Contact New(int contacttype)
    {
      return Csla.DataPortal.CreateChild<Contact>(contacttype);
    }


    #endregion

    #region Data Access

    #region Create

    protected void Child_Create()
    {
      base.Child_Create();
      LoadProperty(_Code, "[Auto-Populated]");
      BusinessRules.CheckRules();
    }

    protected void Child_Create(int contacttype)
    {
      base.Child_Create();
      LoadProperty(_ContactType, contacttype);
      LoadProperty(_Code, "[Auto-Populated]");
      BusinessRules.CheckRules();
    }

    #endregion

    #region Fetch

    private void Child_Fetch(SafeDataReader dr, string columnalias)
    {
      LoadProperty(_Id, dr.GetInt32(columnalias + "contact"));
      LoadProperty(_Code, dr.GetString(columnalias + "code"));
      LoadProperty(_Name, dr.GetString(columnalias + "name"));
      LoadProperty(_ContactType, dr.GetInt32(columnalias + "contacttype"));
      LoadProperty(_Phone, dr.GetString(columnalias + "phone"));
      LoadProperty(_Fax, dr.GetString(columnalias + "fax"));
      LoadProperty(_Email, dr.GetString(columnalias + "email"));

      var schematable = dr.GetSchemaTable();
      if (schematable != null)
      {
        if (schematable.Select("ColumnName='" + columnalias + "firstname" + "'").Length > 0)
          LoadProperty(_FirstName, dr.GetString(columnalias + "firstname"));
        if (schematable.Select("ColumnName='" + columnalias + "middlename" + "'").Length > 0)
          LoadProperty(_MiddleName, dr.GetString(columnalias + "middlename"));
        if (schematable.Select("ColumnName='" + columnalias + "lastname" + "'").Length > 0)
          LoadProperty(_LastName, dr.GetString(columnalias + "lastname"));
      }
    }

    #endregion

    #region Insert

    protected void Child_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO contact(code,name,firstname,middlename,lastname,contacttype,phone,fax,email)
                                        VALUES (@code,@name,@firstname,@middlename,@lastname,@contacttype,@phone,@fax,@email)
                                        SELECT SCOPE_IDENTITY()";
          if (ContactType == BitzConstants.ContactTypes.Employee.Id)
            LoadProperty(_Code, string.Format("ITM{0:yyyy}{0:MM}-{1:00000}", DateTime.Now.Date,
                TableCounterInfo.Get(new SingleCriteria<int>(BitzConstants.TableCounter.Employee.Id)).Counter));
          else if (ContactType == BitzConstants.ContactTypes.Consignee.Id)
            LoadProperty(_Code, string.Format("CON{0:yyyy}{0:MM}-{1:00000}", DateTime.Now.Date,
                TableCounterInfo.Get(new SingleCriteria<int>(BitzConstants.TableCounter.Consignee.Id)).Counter));
          else if (ContactType == BitzConstants.ContactTypes.Vessel.Id)
            LoadProperty(_Code, string.Format("VES{0:yyyy}{0:MM}-{1:00000}", DateTime.Now.Date,
                TableCounterInfo.Get(new SingleCriteria<int>(BitzConstants.TableCounter.Vessel.Id)).Counter));
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@firstname", FirstName);
          cmd.Parameters.AddWithValue("@middlename", MiddleName);
          cmd.Parameters.AddWithValue("@lastname", LastName);
          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@contacttype", ContactType);
          cmd.Parameters.AddWithValue("@phone", Phone);
          cmd.Parameters.AddWithValue("@fax", Fax);
          cmd.Parameters.AddWithValue("@email", Email);
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
    }

    #endregion

    #region Update

    protected void Child_Update()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE contact SET 
                                            code = @code,
                                            name = @name,
                                            firstname = @firstname,
                                            middlename = @middlename,
                                            lastname = @lastname,
                                            contacttype = @contacttype,
                                            phone = @phone,
                                            fax = @fax,
                                            email = @email
                                        WHERE contact = @id";
          cmd.Parameters.AddWithValue("@code", Code);
          cmd.Parameters.AddWithValue("@name", Name);
          cmd.Parameters.AddWithValue("@firstname", FirstName);
          cmd.Parameters.AddWithValue("@middlename", MiddleName);
          cmd.Parameters.AddWithValue("@lastname", LastName);
          cmd.Parameters.AddWithValue("@contacttype", ContactType);
          cmd.Parameters.AddWithValue("@phone", Phone);
          cmd.Parameters.AddWithValue("@fax", Fax);
          cmd.Parameters.AddWithValue("@email", Email);
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
    }

    #endregion

    #endregion
  }

}
