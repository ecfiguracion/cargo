﻿using Bitz.Core.Data;
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
  #region UnitOfMeasureInfo

  [Serializable]
  [TableInfo(TableName = "uom", KeyColumn = "uom.uom")]
  public class UnitOfMeasureInfo : ReadOnlyBase<UnitOfMeasureInfo>
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

    #region Description

    public static readonly PropertyInfo<string> _Description = RegisterProperty<string>(c => c.Description);
    public string Description
    {
      get { return GetProperty(_Description); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Factory Methods

    public static UnitOfMeasureInfo Get(SafeDataReader dr)
    {
      return Csla.DataPortal.FetchChild<UnitOfMeasureInfo>(dr);
    }

    public static void Delete(SingleCriteria<int> id, EventHandler<DataPortalResult<UnitOfMeasureInfo>> completed)
    {
      DataPortal<UnitOfMeasureInfo> dp = new DataPortal<UnitOfMeasureInfo>();
      dp.DeleteCompleted += completed;
      dp.BeginDelete(id);
    }

    #endregion

    #region Data Access

    #region Fetch

    #region Child_Fetch

    private void Child_Fetch(SafeDataReader dr)
    {
      LoadProperty(_Id, dr.GetInt32("uom"));
      LoadProperty(_Code, dr.GetString("code"));
      LoadProperty(_Name, dr.GetString("name"));
      LoadProperty(_Description, dr.GetString("description"));
    }

    #endregion

    #region Delete

    private void DataPortal_Delete(SingleCriteria<int> id)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase()))
      {
        using (var cm = ctx.Connection.CreateCommand())
        {
          cm.CommandText += @"DELETE FROM uom WHERE uom = @id";
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

  #region UnitOfMeasureInfos

  [Serializable]
  public class UnitOfMeasureInfos : ReadOnlyListBase<UnitOfMeasureInfos, UnitOfMeasureInfo>
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

    public static void Get(Criteria criteria, EventHandler<DataPortalResult<UnitOfMeasureInfos>> completed)
    {
      DataPortal<UnitOfMeasureInfos> dp = new DataPortal<UnitOfMeasureInfos>();
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
          cmd.CommandText = @"SELECT uom,code,name,description
                              FROM uom
                              WHERE (code LIKE @SearchText 
                                      OR name LIKE @SearchText
                                      OR description LIKE @SearchText)";
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
              this.Add(UnitOfMeasureInfo.Get(dr));
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
