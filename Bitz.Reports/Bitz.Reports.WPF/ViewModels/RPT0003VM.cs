using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bitz.Reports.ViewModels
{
  public class RPT0003VM : ReportViewModelBase<object>
  {
    public RPT0003VM()
    {

    }

    #region Properties    
    
    private string Parameters
    {
      get
      {
        XElement xml = new XElement("Parameters",
            new XElement("date", DateTime.Now.ToShortDateString())
        );
        return xml.ToString();
      }
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
