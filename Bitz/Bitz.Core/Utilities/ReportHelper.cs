using Bitz.Core.Constants;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bitz.Core.Utilities
{
  public class ReportHelper
  {
    public static void Print(CoreConstants.Report report, int id)
    {
      Print(report.Assembly, CreateIdParameter(id), true);
    }

    public static void Print(string reportcode, string parameterXML)
    {
      Print(reportcode, parameterXML, true);
    }

    public static void Print(string reportcode, string parameterXML, bool preview)
    {
      // Load report dll
      StiReport report = StiReport.GetReportFromAssembly(ConfigHelper.GetReportsPath() + reportcode);

      // Fill up report parameter values
      XDocument xdocument = XDocument.Parse(parameterXML);
      foreach (var param in xdocument.Root.Descendants())
      {
        var paramName = param.Name.LocalName;
        var paramValue = param.Value;
        report[paramName] = paramValue;
      }

      // Override report connection string
      var connectionString = ConfigHelper.GetDatabase();
      report.Dictionary.Databases.Clear();
      report.Dictionary.Databases.Add(new StiSqlDatabase("cn", connectionString));

      if (preview)
        report.ShowWithWpf();
      else
        report.PrintWithWpf(false);
    }

    public static string CreateIdParameter(int id)
    {
      XElement xml = new XElement("Parameters",
          new XElement("pId", id)
      );
      return xml.ToString();
    }
  }
}
