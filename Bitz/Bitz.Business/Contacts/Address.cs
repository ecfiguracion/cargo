using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Business.Contacts
{
  [Serializable]
  public class Address : BusinessBase<Address>
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

    #region Contact

    public static readonly PropertyInfo<int> _Contact = RegisterProperty<int>(c => c.Contact);
    public int Contact
    {
      get { return GetProperty(_Contact); }
      set { SetProperty(_Contact, value); }
    }

    #endregion

    #region AddressName

    public static readonly PropertyInfo<string> _AddressName = RegisterProperty<string>(c => c.AddressName);
    public string AddressName
    {
      get { return GetProperty(_AddressName); }
      set { SetProperty(_AddressName, value); }
    }

    #endregion

    #region Street

    public static readonly PropertyInfo<string> _Street = RegisterProperty<string>(c => c.Street);
    public string Street
    {
      get { return GetProperty(_Street); }
      set { SetProperty(_Street, value); }
    }

    #endregion

    #region City

    public static readonly PropertyInfo<string> _City = RegisterProperty<string>(c => c.City);
    public string City
    {
      get { return GetProperty(_City); }
      set { SetProperty(_City, value); }
    }

    #endregion

    #region Province

    public static readonly PropertyInfo<string> _Province = RegisterProperty<string>(c => c.Province);
    public string Province
    {
      get { return GetProperty(_Province); }
      set { SetProperty(_Province, value); }
    }

    #endregion

    #region State

    public static readonly PropertyInfo<string> _State = RegisterProperty<string>(c => c.State);
    public string State
    {
      get { return GetProperty(_State); }
      set { SetProperty(_State, value); }
    }

    #endregion

    #region Country

    public static readonly PropertyInfo<string> _Country = RegisterProperty<string>(c => c.Country);
    public string Country
    {
      get { return GetProperty(_Country); }
      set { SetProperty(_Country, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(_AddressName));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_AddressName, 300));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Street, 100));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_City, 100));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Province, 100));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_State, 100));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.MaxLength(_Country, 100));
    }

    #endregion

    #region Factory Methods

    public static Address New()
    {
      return Csla.DataPortal.CreateChild<Address>();
    }

    public static Address Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<Address>(dr);
    }

    public static Address Get(SafeDataReader dr, string columnalias)
    {
      return Csla.DataPortal.FetchChild<Address>(dr, columnalias);
    }

    #endregion

    #region Data Access

    #region Fetch

    protected override void Child_Create()
    {
      base.Child_Create();
      BusinessRules.CheckRules();
    }

    private void Child_Fetch(SafeDataReader dr, string columnalias)
    {
      LoadProperty(_Id, dr.GetInt32(columnalias + "contactaddress"));
      LoadProperty(_Contact, dr.GetInt32(columnalias + "contact"));
      LoadProperty(_AddressName, dr.GetString(columnalias + "addressname"));
      LoadProperty(_Street, dr.GetString(columnalias + "street"));
      LoadProperty(_City, dr.GetString(columnalias + "city"));
      LoadProperty(_Province, dr.GetString(columnalias + "province"));
      LoadProperty(_State, dr.GetString(columnalias + "state"));
      LoadProperty(_Country, dr.GetString(columnalias + "country"));
    }

    #endregion

    #region Child Insert

    protected void Child_Insert(SingleCriteria<int> parentId)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO contactaddress(contact,addressname,street,city,province,state,country)
                                        VALUES (@contact,@addressname,@street,@city,@province,@state,@country)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@contact", parentId.Value);
          cmd.Parameters.AddWithValue("@addressname", AddressName);
          cmd.Parameters.AddWithValue("@street", Street);
          cmd.Parameters.AddWithValue("@city", City);
          cmd.Parameters.AddWithValue("@province", Province);
          cmd.Parameters.AddWithValue("@state", State);
          cmd.Parameters.AddWithValue("@country", Country);

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
          cmd.CommandText = @"UPDATE contactaddress SET
                                            contact = @contact,
                                            addressname = @addressname,
                                            street = @street,
                                            city = @city,
                                            province = @province,
                                            state = @state,
                                            country = @country
                                        WHERE contactaddress = @id";
          cmd.Parameters.AddWithValue("@contact", parentId.Value);
          cmd.Parameters.AddWithValue("@addressname", AddressName);
          cmd.Parameters.AddWithValue("@street", Street);
          cmd.Parameters.AddWithValue("@city", City);
          cmd.Parameters.AddWithValue("@province", Province);
          cmd.Parameters.AddWithValue("@state", State);
          cmd.Parameters.AddWithValue("@country", Country);
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
          cmd.CommandText = @"DELETE FROM contactaddress WHERE contactaddress = @id";
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

  [Serializable]
  public class Addresss : BusinessListBase<Addresss, Address>
  {
    #region Factory Methods

    public static Addresss New()
    {
      return new Addresss();
    }

    public static Addresss Get(SingleCriteria<int> id)
    {
      return Csla.DataPortal.Fetch<Addresss>(id);
    }

    #endregion

    #region Data Access

    private void DataPortal_Fetch(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT contactaddress,contact,addressname,street,city,province,state,country
                                        FROM contactaddress
                                        WHERE contact = @contact";
          cmd.Parameters.AddWithValue("@contact", id.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            while (dr.Read())
            {
              this.Add(Address.Get(dr));
            }
          }
        }
      }

    }


    #endregion
  }

}
