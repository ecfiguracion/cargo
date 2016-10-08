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

namespace Bitz.Cargo.Business.Settings
{
  #region BankAccountDetail
  [Serializable]
  public class BankAccountDetail : BusinessBase<BankAccountDetail>
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

    #region BankAccount

    public static readonly PropertyInfo<int> _BankAccount = RegisterProperty<int>(c => c.BankAccount);
    public int BankAccount
    {
      get { return GetProperty(_BankAccount); }
      set { SetProperty(_BankAccount, value); }
    }

    #endregion

    #region BankName

    public static readonly PropertyInfo<string> _BankName = RegisterProperty<string>(c => c.BankName, "Bank Name");
    public string BankName
    {
      get { return GetProperty(_BankName); }
      set { SetProperty(_BankName, value); }
    }

    #endregion

    #region AccountNumber

    public static readonly PropertyInfo<string> _AccountNumber = RegisterProperty<string>(c => c.AccountNumber, "Account Number");
    public string AccountNumber
    {
      get { return GetProperty(_AccountNumber); }
      set { SetProperty(_AccountNumber, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_BankName));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_AccountNumber));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_BankName,300));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_AccountNumber,100));
    }

    #endregion

    #region Factory Methods

    public static BankAccountDetail New()
    {
      return Csla.DataPortal.CreateChild<BankAccountDetail>();
    }

    public static BankAccountDetail Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BankAccountDetail>(dr);
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
      LoadProperty(_Id, dr.GetInt32("bankaccountdetail"));
      LoadProperty(_BankAccount, dr.GetInt32("bankaccount"));
      LoadProperty(_BankName, dr.GetString("bankname"));
      LoadProperty(_AccountNumber, dr.GetString("accountnumber"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO bankaccountdetail(bankaccount,bankname,accountnumber)
                              VALUES (@bankaccount,@bankname,@accountnumber)
                            SELECT SCOPE_IDENTITY()";

          cmd.Parameters.AddWithValue("@bankaccount", parentId.Value);
          cmd.Parameters.AddWithValue("@bankname", BankName);
          cmd.Parameters.AddWithValue("@accountnumber", AccountNumber);

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
          cmd.CommandText = @"UPDATE bankaccountdetail SET 
                                bankname = @bankname,
                                accountnumber = @accountnumber
                              WHERE BankAccountDetail = @id";
          cmd.Parameters.AddWithValue("@bankname", BankName);
          cmd.Parameters.AddWithValue("@accountnumber", AccountNumber);
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
          cmd.CommandText = @"DELETE FROM bankaccountdetail WHERE bankaccountdetail = @id";
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
  #endregion

  #region BankAccountDetails
  [Serializable]
  public class BankAccountDetails : BusinessListBase<BankAccountDetails, BankAccountDetail>
  {
    #region Factory Methods

    public static BankAccountDetails New()
    {
      return new BankAccountDetails();
    }

    public static BankAccountDetails Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<BankAccountDetails>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT bankaccountdetail,bankaccount,bankname,accountnumber
                              FROM bankaccountdetail
                              WHERE bankaccount = @id";
          cmd.Parameters.AddWithValue("@id", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(BankAccountDetail.Get(dr));
            }
          }
        }
      }

    }

    #endregion
  }
  #endregion
}
