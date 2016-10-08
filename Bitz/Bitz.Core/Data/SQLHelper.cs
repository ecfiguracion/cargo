using Csla.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Data
{
  public static class SQLHelper
  {
    public static int GetTotalItemsCount(SqlCommand cmd)
    {
      int total = 0;
      var sqlcommand = cmd.CommandText;

      cmd.CommandText = string.Format(
          @"SET ARITHABORT ON; SELECT COUNT(1)  FROM ({0}) result", cmd.CommandText);

      var dr = cmd.ExecuteReader();
      if (dr.Read())
        total = dr.GetInt32(0);
      dr.Close();

      cmd.CommandText = sqlcommand;

      return total;
    }

    public static void AddSQLPaging(IPageCriteria pageCriteria, string sortby, SqlCommand cmd)
    {
      pageCriteria.TotalItemCount = SQLHelper.GetTotalItemsCount(cmd);
      cmd.CommandText += string.Format(@" ORDER BY {0}
                            OFFSET {1} * ({2} - 1) ROWS
                            FETCH NEXT {1} ROWS ONLY", sortby, pageCriteria.PageSize, pageCriteria.PageIndex);
    }

    public static bool HasColumn(SafeDataReader dr,string columnName) {
      return (dr.GetSchemaTable().Select(string.Format("ColumnName='{0}'", columnName)).Length > 0);
    }

  }
}
