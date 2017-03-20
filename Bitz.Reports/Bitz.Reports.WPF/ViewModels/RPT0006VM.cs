using Bitz.Core.ViewModel;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bitz.Reports.ViewModels
{
  public class RPT0006VM : ReportViewModelBase<object>
  {
    public RPT0006VM()
    {
      StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
      EndDate = StartDate.AddMonths(1).AddDays(-1);
    }

    #region Properties    
    
    private string Parameters
    {
      get
      {
        XElement xml = new XElement("Parameters",
            new XElement("startdate", StartDate.Date.ToString("yyyy-MM-dd")),
            new XElement("enddate", EndDate.Date.ToString("yyyy-MM-dd"))
        );
        return xml.ToString();
      }
    }

    public DateTime StartDate
    {
      get;
      set;
    }

    public DateTime EndDate
    {
      get;
      set;
    }

    #endregion

    #region Commands

    public override void CommandRunExecute(object parameter)
    {
      base.CommandRunExecute(this.Parameters);
    }

    #endregion
  }
}
