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

    #endregion

    #region One To Many Properties

    #region Addresss

    public static readonly PropertyInfo<Addresss> _Addresss = RegisterProperty<Addresss>(c => c.Addresss);
    public Addresss Addresss
    {
      get { return GetProperty(_Addresss); }
      set { SetProperty(_Addresss, value); }
    }

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
      LoadProperty(_Addresss, Addresss.New());
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
          cmd.CommandText = string.Format(@"SELECT s.consignee,s.contact,
                                              c.contact as {0}contact,c.code as {0}code,c.name as {0}name,
                                              c.contacttype as {0}contacttype,c.phone as {0}phone,
                                              c.fax as {0}fax,c.email as {0}email                                      
                                            FROM consignee s
                                            INNER JOIN contact c ON s.contact = c.contact
                                            WHERE s.consignee = @id", _Contact.Name);
          cmd.Parameters.AddWithValue("@id", id);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            if (dr.Read())
            {
              LoadProperty(_Id, dr.GetInt32("consignee"));
              LoadProperty(_Contact, Contact.Get(dr, _Contact.Name));

            }
          }
        }
      }
      this.ChildFetch();
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
          cmd.CommandText = @"INSERT INTO consignee(contact)
                                        VALUES (@contact)
                                        SELECT SCOPE_IDENTITY()";
          cmd.Parameters.AddWithValue("@contact", Contact.Id);
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
                                            contact = @contact
                                        WHERE consignee = @id";
          cmd.Parameters.AddWithValue("@contact", Contact.Id);
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
      LoadProperty(_Addresss, Addresss.Get(new SingleCriteria<int>(this.Contact.Id)));
    }

    #endregion

    #region SaveChild

    private void SaveChild()
    {
      Csla.DataPortal.UpdateChild(ReadProperty(_Addresss), new SingleCriteria<int>(this.Contact.Id));
    }

    #endregion

    #endregion
  }
}
