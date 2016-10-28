using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.ViewModels.Report
{
  public class ReportManagerVM : GridViewModelBase<object>
  {
    public ReportManagerVM()
    {
      this.Refresh();
    }

    #region Properties

    private string _SearchText = string.Empty;
    public string SearchText
    {
      get { return _SearchText; }
      set { _SearchText = value; }
    }

    #endregion

    #region Commands

    public override void CommandRefreshExecute(object parameter)
    {
      this.Refresh();
    }

    public override void CommandOpenExecute(object parameter)
    {
      var report = this.SelectedItem as CoreConstants.Report;
      if (report != null)
      {
        NavigationManager.Show(report.UserInterface,new object[] { report });
      }
    }

    #endregion

    #region Methods

    private void Refresh()
    {
      this.Model = Reports.Get(this.SearchText).OrderBy(x => x.Code);
    }

    #endregion

  }
}
