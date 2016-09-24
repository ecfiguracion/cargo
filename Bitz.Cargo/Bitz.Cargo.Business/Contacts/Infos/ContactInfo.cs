using Bitz.Core.Data;
using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Cargo.Business.Contacts.Infos
{
  #region ContactInfo

  [Serializable]
  [TableInfo(TableName = "contact", KeyColumn = "contact.contact")]
  public class ContactInfo : ReadOnlyBase<ContactInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region ContactType

    public static readonly PropertyInfo<int> _ContactType = RegisterProperty<int>(c => c.ContactType);
    public int ContactType
    {
      get { return GetProperty(_ContactType); }
    }

    #endregion

    #region Code

    public static readonly PropertyInfo<string> _Code = RegisterProperty<string>(c => c.Code);
    public string Code
    {
      get { return GetProperty(_Code); }
    }

    #endregion

    #region Name

    public static readonly PropertyInfo<string> _Name = RegisterProperty<string>(c => c.Name);
    public string Name
    {
      get { return GetProperty(_Name); }
    }

    #endregion

    #region Phone

    public static readonly PropertyInfo<string> _Phone = RegisterProperty<string>(c => c.Phone);
    public string Phone
    {
      get { return GetProperty(_Phone); }
    }

    #endregion

    #region Fax

    public static readonly PropertyInfo<string> _Fax = RegisterProperty<string>(c => c.Fax);
    public string Fax
    {
      get { return GetProperty(_Fax); }
    }

    #endregion

    #region Email

    public static readonly PropertyInfo<string> _Email = RegisterProperty<string>(c => c.Email);
    public string Email
    {
      get { return GetProperty(_Email); }
    }

    #endregion

    #region Firstname

    public static readonly PropertyInfo<string> _Firstname = RegisterProperty<string>(c => c.Firstname);
    public string Firstname
    {
      get { return GetProperty(_Firstname); }
    }

    #endregion

    #region MiddleName

    public static readonly PropertyInfo<string> _MiddleName = RegisterProperty<string>(c => c.MiddleName);
    public string MiddleName
    {
      get { return GetProperty(_MiddleName); }
    }

    #endregion

    #region Lastname

    public static readonly PropertyInfo<string> _Lastname = RegisterProperty<string>(c => c.Lastname);
    public string Lastname
    {
      get { return GetProperty(_Lastname); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static ContactInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ContactInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<ContactInfo>> completed)
    {
      DataPortal<ContactInfo> dp = new DataPortal<ContactInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("contact"));
      LoadProperty(_ContactType, dr.GetInt32("contacttype"));
      LoadProperty(_Code, dr.GetString("code"));
      LoadProperty(_Name, dr.GetString("name"));
      LoadProperty(_Phone, dr.GetString("phone"));
      LoadProperty(_Fax, dr.GetString("fax"));
      LoadProperty(_Email, dr.GetString("email"));
      LoadProperty(_Firstname, dr.GetString("firstname"));
      LoadProperty(_MiddleName, dr.GetString("middlename"));
      LoadProperty(_Lastname, dr.GetString("lastname"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM contact WHERE contact = @id";
          cm.Parameters.AddWithValue("@id", id.Value);
          cm.ExecuteNonQuery();
        }
      }
    }

    #endregion

    #endregion

    #endregion
  }

  #endregion

  #region ContactInfos

  [Serializable]
  public class ContactInfos : ReadOnlyListBase<ContactInfos, ContactInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : CriteriaBase<Criteria>
    {
      #region SearchText
      private static PropertyInfo<string> _SearchText = RegisterProperty<string>(c => c.SearchText);

      public string SearchText
      {
        get { return ReadProperty(_SearchText); }
        set { LoadProperty(_SearchText, value); }
      }
      #endregion //SearchText

      #region ContactType
      private static PropertyInfo<int?> _ContactType = RegisterProperty<int?>(c => c.ContactType);

      public int? ContactType
      {
        get { return ReadProperty(_ContactType); }
        set { LoadProperty(_ContactType, value); }
      }
      #endregion //ItemType

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<ContactInfos>> completed)
    {
      DataPortal<ContactInfos> dp = new DataPortal<ContactInfos>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(criteria);
    }

    #endregion

    #region Data Access

    protected void DataPortal_Fetch(Criteria criteria)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = @"SELECT contact
                                  ,code
                                  ,name
                                  ,contacttype
                                  ,phone
                                  ,fax
                                  ,email
                                  ,firstname
                                  ,middlename
                                  ,lastname
                              FROM contact
                              WHERE (code LIKE @SearchText 
                                  OR name LIKE @SearchText 
                                  OR phone LIKE @SearchText 
                                  OR fax LIKE @SearchText 
                                  OR email LIKE @SearchText 
                                  OR firstname LIKE @SearchText 
                                  OR middlename LIKE @SearchText 
                                  OR lastname LIKE @SearchText)
                                AND (contacttype = @contactType OR @contactType IS NULL)";

          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          if (criteria.ContactType != null)
            cmd.Parameters.AddWithValue("@contactType", criteria.ContactType);
          else
            cmd.Parameters.AddWithValue("@contactType", DBNull.Value);

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(ContactInfo.Get(dr));
            }
            IsReadOnly = true;
          }
        }
      }
    }

    #endregion

  }

  #endregion
}
