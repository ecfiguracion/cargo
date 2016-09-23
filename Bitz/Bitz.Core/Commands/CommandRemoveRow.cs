using System.Data.SqlClient;
using Bitz.Core.Utilities;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Data;

namespace Bitz.Core.Commands
{
  [Serializable()]
  public class CommandRemoveRow : CommandBase<CommandRemoveRow>
  {
    #region Properties

    #region TableName

    public static readonly PropertyInfo<string> _TableName = RegisterProperty<string>(c => c.TableName);
    public string TableName
    {
      get { return ReadProperty(_TableName); }
    }

    #endregion

    #region KeyColumn

    public static readonly PropertyInfo<string> _KeyColumn = RegisterProperty<string>(c => c.KeyColumn);
    public string KeyColumn
    {
      get { return ReadProperty(_KeyColumn); }
    }

    #endregion

    #region KeyColumnValue

    public static readonly PropertyInfo<int> _KeyColumnValue = RegisterProperty<int>(c => c.KeyColumnValue);
    public int KeyColumnValue
    {
      get { return ReadProperty(_KeyColumnValue); }
    }

    #endregion

    #endregion

    #region Factory Methods

    public static void Execute(string tablename, string keycolumn, int keycolumnvalue,
        EventHandler<DataPortalResult<CommandRemoveRow>> completed)
    {
      var cmd = new CommandRemoveRow();
      cmd.LoadProperty(_TableName, tablename);
      cmd.LoadProperty(_KeyColumn, keycolumn);
      cmd.LoadProperty(_KeyColumnValue, keycolumnvalue);
      Csla.DataPortal.BeginExecute<CommandRemoveRow>(cmd, completed);
    }

    #endregion

    #region Data Access

    protected override void DataPortal_Execute()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName, KeyColumn, KeyColumnValue);
          cmd.ExecuteNonQuery();
        }
      }

    }

    #endregion



  }
}
