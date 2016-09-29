using Bitz.Core.Utilities;
using Csla;
using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Business.Settings.Infos
{
  #region TableCounterInfo

  [Serializable]
  public class TableCounterInfo : ReadOnlyBase<TableCounterInfo>
  {
    #region One To One Properties

    #region Id

    public static readonly PropertyInfo<int> _Id = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(_Id); }
    }

    #endregion

    #region TableId

    public static readonly PropertyInfo<int> _TableId = RegisterProperty<int>(c => c.TableId);
    public int TableId
    {
      get { return GetProperty(_TableId); }
    }

    #endregion

    #region Counter

    public static readonly PropertyInfo<int> _Counter = RegisterProperty<int>(c => c.Counter);
    public int Counter
    {
      get { return GetProperty(_Counter); }
    }

    #endregion

    #endregion

    #region One To Many Properties

    #endregion

    #region Criteria

    [Serializable]
    public class Criteria : CriteriaBase<Criteria>
    {
      #region TableId
      private static PropertyInfo<int> _TableId = RegisterProperty<int>(c => c.TableId);

      public int TableId
      {
        get { return ReadProperty(_TableId); }
        set { LoadProperty(_TableId, value); }
      }
      #endregion //TableId

      #region IncrementCounter
      private static PropertyInfo<bool> _IncrementCounter = RegisterProperty<bool>(c => c.IncrementCounter);

      public bool IncrementCounter
      {
        get { return ReadProperty(_IncrementCounter); }
        set { LoadProperty(_IncrementCounter, value); }
      }
      #endregion //IncrementCounter

      #region RetrieveCounter
      private static PropertyInfo<bool> _RetrieveCounter = RegisterProperty<bool>(c => c.RetrieveCounter);

      public bool RetrieveCounter
      {
        get { return ReadProperty(_RetrieveCounter); }
        set { LoadProperty(_RetrieveCounter, value); }
      }
      #endregion //RetrieveCounter
    }

    #endregion

    #region Factory Methods

    public static void Get(SingleCriteria<int> tableid, EventHandler<DataPortalResult<TableCounterInfo>> completed)
    {
      DataPortal<TableCounterInfo> dp = new DataPortal<TableCounterInfo>();
      dp.FetchCompleted += completed;
      dp.BeginFetch(tableid);
    }

    public static TableCounterInfo Get(Criteria criteria)
    {
      return Csla.DataPortal.Fetch<TableCounterInfo>(criteria);
    }

    public static TableCounterInfo Get(SingleCriteria<int> tableid)
    {
      return Csla.DataPortal.Fetch<TableCounterInfo>(tableid);
    }

    #endregion

    #region Data Access

    #region Fetch

    private void DataPortal_Fetch(SingleCriteria<int> tableId)
    {
      this.DataPortal_Fetch(new Criteria() { TableId = tableId.Value, RetrieveCounter = true, IncrementCounter = true });
    }

    private void DataPortal_Fetch(Criteria criteria)
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          if (criteria.RetrieveCounter)
          {
            cmd.CommandText = @"SELECT tablecounter,tableid,counter
                                        FROM tablecounter
                                        WHERE tableid = @tableid";

            cmd.Parameters.AddWithValue("@tableid", criteria.TableId);

            using (var dr = new SafeDataReader(cmd.ExecuteReader()))
            {
              if (dr.Read())
              {
                LoadProperty(_Id, dr.GetInt32("tablecounter"));
                LoadProperty(_TableId, dr.GetInt32("tableid"));
                if (criteria.IncrementCounter)
                  LoadProperty(_Counter, dr.GetInt32("counter") + 1);
                else
                  LoadProperty(_Counter, dr.GetInt32("counter"));
              }
            }
          }

          if (this.Id == 0 && criteria.RetrieveCounter)
          {
            cmd.Parameters.Clear();
            cmd.CommandText = @"INSERT INTO tablecounter(tableid,counter)
                                            VALUES (@tableid,@counter)
                                            SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@tableid", criteria.TableId);
            cmd.Parameters.AddWithValue("@counter", 1);

            try
            {
              int identity = Convert.ToInt32(cmd.ExecuteScalar());
              LoadProperty(_Id, identity);
              LoadProperty(_TableId, criteria.TableId);
              LoadProperty(_Counter, 1);
            }
            catch (Exception ex)
            {
              throw ex;
            }
            finally
            {

            }
          }
          else
          {
            if (criteria.IncrementCounter)
            {
              cmd.Parameters.Clear();
              cmd.CommandText = @"UPDATE tablecounter SET counter = counter + 1 
                                                WHERE tableid = @tableid";
              cmd.Parameters.AddWithValue("@tableid", criteria.TableId);

              try
              {
                cmd.ExecuteNonQuery();
              }
              catch (Exception ex)
              {
                throw ex;
              }
              finally
              {

              }
            }
          }
        }
      }

    }

    #endregion

    #endregion
  }

  #endregion
}
