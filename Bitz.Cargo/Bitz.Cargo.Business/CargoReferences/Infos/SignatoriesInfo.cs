using Bitz.Cargo.Business.Constants;
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

namespace Bitz.Cargo.Business.CargoReferences.Infos
{
  #region SignatoriesInfo

  [Serializable]
  [TableInfo(TableName = "signatory", KeyColumn = "signatory.signatory")]
  public class SignatoriesInfo : ReadOnlyBase<SignatoriesInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region Type

    public static readonly PropertyInfo<int> _Type = RegisterProperty<int>(c => c.Type);
    public CoreConstants.IdValue Type
    {
      get 
      {
        var type = GetProperty(_Type);
        return CargoConstants.SignatoryTypes.Items.SingleOrDefault(x => x.Id == type);
      }
    }

    #endregion

    #region Name

    public static readonly PropertyInfo<string> _Name = RegisterProperty<string>(c => c.Name);
    public string Name
    {
      get { return GetProperty(_Name); }
    }

    #endregion

    #region Position

    public static readonly PropertyInfo<string> _Position = RegisterProperty<string>(c => c.Position);
    public string Position
    {
      get { return GetProperty(_Position); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static SignatoriesInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<SignatoriesInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<SignatoriesInfo>> completed)
    {
      DataPortal<SignatoriesInfo> dp = new DataPortal<SignatoriesInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("signatory"));
      LoadProperty(_Type, dr.GetInt32("type"));
      LoadProperty(_Name, dr.GetString("name"));
      LoadProperty(_Position, dr.GetString("position"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM signatory WHERE signatory = @id";
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

  #region SignatoriesInfos

  [Serializable]
  public class SignatoriesInfos : ReadOnlyListBase<SignatoriesInfos, SignatoriesInfo>
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

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<SignatoriesInfos>> completed)
    {
      DataPortal<SignatoriesInfos> dp = new DataPortal<SignatoriesInfos>();
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
          cmd.CommandText = @"SELECT signatory,type,name,position
                              FROM signatory
                              WHERE (name LIKE @SearchText)";
          cmd.Parameters.AddWithValue("@SearchText", "%" + criteria.SearchText + "%");

          //Apply paging
          if (criteria.PageSize > 0)
          {
            var sortby = "name ASC";
            SQLHelper.AddSQLPaging(criteria, sortby, cmd);
          }

          using (var dr = new SafeDataReader(cmd.ExecuteReader()))
          {
            IsReadOnly = false;
            while (dr.Read())
            {
              this.Add(SignatoriesInfo.Get(dr));
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
