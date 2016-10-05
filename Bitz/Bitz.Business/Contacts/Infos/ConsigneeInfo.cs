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
  #region ConsigneeInfo

  [Serializable]
  [TableInfo(TableName = "consignee", KeyColumn = "consignee.consignee")]
  public class ConsigneeInfo : ReadOnlyBase<ConsigneeInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Contact

    public static readonly PropertyInfo<int> _Contact = RegisterProperty<int>(c => c.Contact);
    public int Contact
    {
      get { return GetProperty(_Contact); }
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

    #region PhoneNumber

    public static readonly PropertyInfo<string> _PhoneNumber = RegisterProperty<string>(c => c.PhoneNumber);
    public string PhoneNumber
    {
      get { return GetProperty(_PhoneNumber); }
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

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static ConsigneeInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<ConsigneeInfo>(dr);
    }

    #endregion

    #region Data Access

    #region Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("consignee"));
      LoadProperty(_Contact, dr.GetInt32("contact"));
      LoadProperty(_Name, dr.GetString("name"));
      LoadProperty(_Code, dr.GetString("code"));
      LoadProperty(_PhoneNumber, dr.GetString("phone"));
      LoadProperty(_Fax, dr.GetString("fax"));
      LoadProperty(_Email, dr.GetString("email"));
    }

    #endregion

    #endregion
  }

  #endregion

  #region ConsigneeInfos

  [Serializable]
  public class ConsigneeInfos : ReadOnlyListBase<ConsigneeInfos, ConsigneeInfo>
  {
    #region Criteria

    [Serializable]
    public class Criteria : PageCriteriaBase<Criteria>
    {
      #region SearchText
      private static PropertyInfo<string> _SearchText = RegisterProperty<string>(c => c.SearchText);

      public string SearchText
      {
        get { return ReadProperty(_SearchText); }
        set { LoadProperty(_SearchText, value); }
      }
      #endregion //SearchText

    }

    #endregion

    #region Factory Methods

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<ConsigneeInfos>> completed)
    {
      DataPortal<ConsigneeInfos> dp = new DataPortal<ConsigneeInfos>();
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
          cmd.CommandText = @"SELECT s.consignee,s.contact,
                                  c.code,c.name,c.phone,c.fax,c.email
                              FROM consignee s
                              INNER JOIN contact c ON s.contact = c.contact
                              WHERE c.name LIKE @SearchText ";

          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "c.name ASC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(ConsigneeInfo.Get(dr));
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
