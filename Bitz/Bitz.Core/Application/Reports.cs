using Bitz.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Application
{
  public class Reports
  {
    static Reports()
    {
      Cargo.Initialise();
    }

    #region Items

    private static List<CoreConstants.Report> _Items = new List<CoreConstants.Report>();
    private static List<CoreConstants.Report> Items
    {
      get { return _Items; }
      set { _Items = value; }
    }

    #endregion

    #region Cargo

    public static class Cargo
    {
      public static CoreConstants.Report RPT0001 { get { return FindById(1100); } }
      public static CoreConstants.Report RPT0002 { get { return FindById(1101); } }
      public static CoreConstants.Report RPT0003 { get { return FindById(1102); } }

      public static void Initialise()
      {
        Items.Add(new CoreConstants.Report(1100, "RPT0001", "Statement of Account", "Print statement of accounts.", "RPT0001.DLL"));
        Items.Add(new CoreConstants.Report(1101, "RPT0002", "Statement of Account - Roro", "Print RORO statement of accounts.", "RPT0002.DLL"));
        Items.Add(new CoreConstants.Report(1102, "RPT0003", "PPA Due", "Print PPA Dues by date range", "RPT0003.DLL",UserInterfaces.Reports.RPT0003View));
      }
    }

    #endregion

    #region Utilities

    #region FindById

    public static CoreConstants.Report FindById(int id)
    {
      CoreConstants.Report report = null;
      foreach (var item in Items)
      {
        if (item.Id == id)
        {
          report = item;
          break;
        }
      }
      return report;
    }

    #endregion

    #region FindByName

    public static CoreConstants.Report FindByName(string name)
    {
      CoreConstants.Report report = null;
      foreach (var item in Items)
      {
        if (item.Name == name)
        {
          report = item;
          break;
        }
      }
      return report;
    }

    #endregion

    #region Get

    public static List<CoreConstants.Report> Get(string name)
    {
      var reports = new List<CoreConstants.Report>();
      foreach (var item in Items)
      {
        if (item.UserInterface != null && (item.Name.ToLower().Contains(name.ToLower()) || item.Code.ToLower().Contains(name.ToLower()) || item.Description.ToLower().Contains(name.ToLower())))
        {
          reports.Add(item);
        }
      }
      return reports;
    }

    #endregion

    #endregion
  }
}
