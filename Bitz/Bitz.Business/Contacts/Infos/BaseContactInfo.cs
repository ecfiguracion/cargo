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

    #region For Student

    #region Course

    public static readonly PropertyInfo<string> _Course = RegisterProperty<string>(c => c.Course);
    public string Course
    {
      get { return GetProperty(_Course); }
    }

    #endregion

    #region YearLevel

    public static readonly PropertyInfo<string> _YearLevel = RegisterProperty<string>(c => c.YearLevel);
    public string YearLevel
    {
      get { return GetProperty(_YearLevel); }
    }

    #endregion

    #endregion

    #endregion

    #region Derived Properties

    #region ContactName

    public string ContactName
    {
      get
      {
        return string.Format("{0} {1}", this.Code, this.Name);
      }
    }

    public override string ToString()
    {
      return this.Name;
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
      LoadProperty(_Id, dr.GetInt32(columnprefix + "contact"));
      LoadProperty(_Code, dr.GetString(columnprefix + "code"));
      LoadProperty(_Name, dr.GetString(columnprefix + "name"));

      var schematable = dr.GetSchemaTable();
      if (schematable != null)
      {
        //for student 
        if (schematable.Select("ColumnName='" + columnprefix + "course" + "'").Length > 0)
          LoadProperty(_Course, dr.GetString(columnprefix + "course"));
        if (schematable.Select("ColumnName='" + columnprefix + "yearlevel" + "'").Length > 0)
          LoadProperty(_YearLevel, dr.GetString(columnprefix + "yearlevel"));
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
          cmd.CommandText = @"SELECT contact,code,name 
                              FROM contact
                              WHERE (contacttype = @contacttype OR @contacttype IS NULL)
                              AND (contact = @contact OR @contact IS NULL)
                              AND (code LIKE @SearchText OR name LIKE @SearchText)        
                              ORDER BY name";

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
