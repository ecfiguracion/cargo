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

namespace Bitz.Cargo.Business.Settings
{
  [Serializable]
  public class BankAccount : BusinessBase<BankAccount>
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

    #region AccountName

    public static readonly PropertyInfo<string> _AccountName = RegisterProperty<string>(c => c.AccountName, "Account Name");
    public string AccountName
    {
      get { return GetProperty(_AccountName); }
      set { SetProperty(_AccountName, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #region BankAccountDetails

    public static readonly PropertyInfo<BankAccountDetails> _BankAccountDetails = RegisterProperty<BankAccountDetails>(c => c.BankAccountDetails);
    public BankAccountDetails BankAccountDetails
    {
      get { return GetProperty(_BankAccountDetails); }
      set { SetProperty(_BankAccountDetails, value); }
    }

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_AccountName));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_AccountName,100));
    }

    #endregion

    #region Factory Methods

    public static void Get(EventHandler<DataPortalResult<BankAccount>> completed)
    {
      DataPortal<BankAccount> dp = new DataPortal<BankAccount>();
      dp.FetchCompleted += completed;
      dp.BeginFetch();
    }

    public static void New(EventHandler<DataPortalResult<BankAccount>> completed)
    {
      Csla.DataPortal.BeginCreate(completed);
    }

    #endregion

    #region Data Access

    #region Create

    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
      LoadProperty(_BankAccountDetails, BankAccountDetails.New());
      this.BusinessRules.CheckRules();
    }

    #endregion

    #region Fetch

    private void DataPortal_Fetch()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT bankaccount,accountname
                              FROM bankaccount";
          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("bankaccount"));
              LoadProperty(_AccountName, dr.GetString("accountname"));
            }
          }
        }
      }
      LoadProperty(_BankAccountDetails, BankAccountDetails.Get(new SingleCriteria<int>(this.Id)));
      BusinessRules.CheckRules();
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO bankaccount(accountname) VALUES (@accountname)
                              SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@accountname", AccountName);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_BankAccountDetails), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      if (this.Id == 0)
      {
        this.DataPortal_Insert();
        return;
      }

      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE bankaccount SET accountname = @accountname
                              WHERE bankaccount = @id";
          cmd.Parameters.AddWithValue("@accountname", AccountName);
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
      Csla.DataPortal.UpdateChild(ReadProperty(_BankAccountDetails), new SingleCriteria<int>(this.Id));
    }

    #endregion

    #endregion
  }

}
