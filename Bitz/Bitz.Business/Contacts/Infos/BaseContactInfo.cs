using Bitz.Core.Constants;
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

namespace Bitz.Business.Contacts.Infos
{
  #region BaseContactInfo

  [Serializable]
  public class BaseContactInfo : ReadOnlyBase<BaseContactInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
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

    #region Address

    public static readonly PropertyInfo<string> _Address = RegisterProperty<string>(c => c.Address);
    public string Address
    {
      get { return GetProperty(_Address); }
    }

    #endregion

    #endregion

    #region Derived Properties

    #region ToString

    public override string ToString()
    {
      return this.Name;
    }

    #endregion

    #region DisplayText

    public string DisplayText
    {
      get
      {
        return string.Format("{0} {1}", this.Code, this.Name);
      }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static BaseContactInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<BaseContactInfo>(dr, string.Empty);
    }

    public static BaseContactInfo Get(SafeDataReader dr, string columnprefix)
    {
      return Csla.DataPortal.FetchChild<BaseContactInfo>(dr, columnprefix);
    }

    #endregion

    #region Data Access

    #region Fetch

    private void Child_Fetch(SafeDataReader dr, string columnprefix)
    {
      //Minimum set of columns
      LoadProperty(_Id, dr.GetInt32(columnprefix + "contact"));
      LoadProperty(_Code, dr.GetString(columnprefix + "code"));
      LoadProperty(_Name, dr.GetString(columnprefix + "name"));

      //Extended column sets
      if (SQLHelper.HasColumn(dr,"Address"))
      {
        LoadProperty(_Address, dr.GetString(columnprefix + "address"));
      }
    }

    #endregion

    #endregion
  }

  #endregion

  #region BaseContactInfos

  [Serializable]
  public class BaseContactInfos : ReadOnlyListBase<BaseContactInfos, BaseContactInfo>
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
      #endregion //ContactType

      #region Contact
      private static PropertyInfo<int?> _Contact = RegisterProperty<int?>(c => c.Contact);

      public int? Contact
      {
        get { return ReadProperty(_Contact); }
        set { LoadProperty(_Contact, value); }
      }
      #endregion //Contact

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<BaseContactInfos>> completed)
    {
      DataPortal<BaseContactInfos> dp = new DataPortal<BaseContactInfos>();
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
          cmd.CommandText = @"SELECT c.contact,c.code,c.name,ca.addressname as address
                              FROM contact c
                              LEFT JOIN contactaddress ca ON c.contact = ca.contact
                              WHERE (c.contacttype = @contacttype OR @contacttype IS NULL)
                              AND (c.contact = @contact OR @contact IS NULL)
                              AND (c.code LIKE @SearchText OR name LIKE @SearchText)        
                              ORDER BY c.name";

          if (criteria.ContactType != null)
            cmd.Parameters.AddWithValue("@contacttype", criteria.ContactType);
          else
            cmd.Parameters.AddWithValue("@contacttype", DBNull.Value);
          if (criteria.Contact != null)
            cmd.Parameters.AddWithValue("@contact", criteria.Contact);
          else
            cmd.Parameters.AddWithValue("@contact", DBNull.Value);
          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(BaseContactInfo.Get(dr));
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
