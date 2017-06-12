using System.Data.SqlClient;
using Bitz.Core.Utilities;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Data;

namespace Bitz.Cargo.Business.Commands
{
  [Serializable()]
  public class CommandUniqueORNumber : CommandBase<CommandUniqueORNumber>
  {
    #region Properties

    #region ORNumber

    public static readonly PropertyInfo<string> _ORNumber = RegisterProperty<string>(c => c.ORNumber);
    public string ORNumber
    {
      get { return ReadProperty(_ORNumber); }
    }

    #endregion

    #region BillId

    public static readonly PropertyInfo<int> _BillId = RegisterProperty<int>(c => c.BillId);
    public int BillId
    {
      get { return ReadProperty(_BillId); }
    }

    #endregion

    #region IsValid

    public static readonly PropertyInfo<bool> _IsValid = RegisterProperty<bool>(c => c.IsValid);
    public bool IsValid
    {
      get { return ReadProperty(_IsValid); }
    }

    #endregion

    #endregion

    #region Factory Methods

    public static void Execute(int billid, string ornumber,
        EventHandler<DataPortalResult<CommandUniqueORNumber>> completed)
    {
      var cmd = new CommandUniqueORNumber();
      cmd.LoadProperty(_BillId, billid);
      cmd.LoadProperty(_ORNumber, ornumber);
      Csla.DataPortal.BeginExecute<CommandUniqueORNumber>(cmd, completed);
    }

    #endregion

    #region Data Access

    protected override void DataPortal_Execute()
    {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(ConfigHelper.GetDatabase(), false))
      {
        using (var cmd = ctx.Connection.CreateCommand())
        {
          cmd.CommandText = string.Format("SELECT 1 FROM bill WHERE bill != {0} And ornumber = '{1}'", BillId, ORNumber);
          var dr = cmd.ExecuteReader();

          var hasrow = dr.Read();
          dr.Close();
          LoadProperty(_IsValid, !hasrow);
        }
      }

    }

    #endregion



  }
}
