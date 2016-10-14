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

namespace Bitz.Business.Contacts
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

    #region Contact

    public static readonly PropertyInfo<Contact> _Contact = RegisterProperty<Contact>(c => c.Contact);
    public Contact Contact
    {
      get { return GetProperty(_Contact); }
      set { SetProperty(_Contact, value); }
    }

    #endregion

    #region Address

    public static readonly PropertyInfo<Address> _Address = RegisterProperty<Address>(c => c.Address);
    public Address Address
    {
      get { return GetProperty(_Address); }
      set { SetProperty(_Address, value); }
    }

    #endregion

    #region TIN

    public static readonly PropertyInfo<string> _TIN = RegisterProperty<string>(c => c.TIN);
    public string TIN
    {
      get { return GetProperty(_TIN); }
      set { SetProperty(_TIN, value); }
    }

    #endregion

    #region WTaxRate

    public static readonly PropertyInfo<decimal> _WTaxRate = RegisterProperty<decimal>(c => c.WTaxRate);
    public decimal WTaxRate
    {
      get { return GetProperty(_WTaxRate); }
      set { SetProperty(_WTaxRate, value); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #region Addresss

    //public static readonly PropertyInfo<Addresss> _Addresss = RegisterProperty<Addresss>(c => c.Addresss);
    //public Addresss Addresss
    //{
    //  get { return GetProperty(_Addresss); }
    //  set { SetProperty(_Addresss, value); }
    //}

    #endregion

    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
    }

    #endregion

    #region Factory Methods

    public static Consignee Get(SafeDataReader dr, string columnalias)
    {
      return Csla.DataPortal.FetchChild<Consignee>(dr, columnalias);
    }

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
      var contact = Contact.New();
      contact.ContactType = BitzConstants.ContactTypes.Consignee.Id;
      LoadProperty(_Contact, contact);
      LoadProperty(_Address, Address.New());
      BusinessRules.CheckRules();
    }

    #endregion

    #region Fetch

    private void DataPortal_Fetch(int id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = string.Format(@"SELECT s.consignee,s.contact,s.wtaxrate,s.tinnumber,
                                              c.contact as {0}contact,c.code as {0}code,c.name as {0}name,
                                              c.contacttype as {0}contacttype,c.phone as {0}phone,
                                              c.fax as {0}fax,c.email as {0}email,a.contact as {1}contact,                                      
                                              a.contactaddress as {1}contactaddress,a.addressname as {1}addressname,a.street as {1}street,
                                              a.city as {1}city,a.province as {1}province,a.state as {1}state,a.country as {1}country
                                            FROM consignee s
                                            INNER JOIN contact c ON s.contact = c.contact
                                            LEFT JOIN contactaddress a ON a.contact = c.contact
                                            WHERE s.consignee = @id", _Contact.Name, _Address.Name);
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("consignee"));
              LoadProperty(_WTaxRate, dr.GetDecimal("wtaxrate"));
              LoadProperty(_TIN, dr.GetString("tinnumber"));
              LoadProperty(_Contact, Contact.Get(dr, _Contact.Name));
              LoadProperty(_Address, Address.Get(dr, _Address.Name));

            }
          }
        }
      }
      //this.ChildFetch();
    }

    #endregion

    #region Insert

    protected override void DataPortal_Insert()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_Contact));
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"INSERT INTO consignee(contact,wtaxrate,tinnumber)
                                        VALUES (@contact,@wtaxrate,@tinnumber)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@contact", Contact.Id);
          cmd.Parameters.AddWithValue("@wtaxrate", WTaxRate);
          cmd.Parameters.AddWithValue("@tinnumber", TIN);
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
      this.SaveChild();
    }

    #endregion

    #region Update

    protected override void DataPortal_Update()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_Contact));
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"UPDATE consignee SET 
                                            contact = @contact,
                                            wtaxrate = @wtaxrate,
                                            tinnumber = @tinnumber
                                        WHERE consignee = @id";
          cmd.Parameters.AddWithValue("@contact", Contact.Id);
          cmd.Parameters.AddWithValue("@wtaxrate", WTaxRate);
          cmd.Parameters.AddWithValue("@tinnumber", TIN);
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
      this.SaveChild();
    }

    #endregion

    #region ChildFetch

    private void ChildFetch()
    {
      LoadProperty(_Address, Addresss.Get(new SingleCriteria<int>(this.Contact.Id)));
    }

    #endregion

    #region SaveChild

    private void SaveChild()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_Address), new SingleCriteria<int>(this.Contact.Id));
    }

    #endregion

    #endregion
  }
}
