using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Data
{
  [AttributeUsage(AttributeTargets.Class)]
  public class TableInfoAttribute : System.Attribute
  {
    public string TableName { get; set; }
    public string KeyColumn { get; set; }
  }
}
