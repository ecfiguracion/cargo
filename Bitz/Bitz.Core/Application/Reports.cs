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
      public static CoreConstants.Report RPT0004 { get { return FindById(1103); } }
      public static CoreConstants.Report RPT0005 { get { return FindById(1104); } }
      public static CoreConstants.Report RPT0006 { get { return FindById(1105); } }
      public static CoreConstants.Report RPT0007 { get { return FindById(1106); } }
      public static CoreConstants.Report RPT0008 { get { return FindById(1107); } }
      public static CoreConstants.Report RPT0009 { get { return FindById(1108); } }
      public static CoreConstants.Report RPT0010 { get { return FindById(1109); } }
      public static CoreConstants.Report RPT0011 { get { return FindById(1110); } }

      public static void Initialise()
      {
        Items.Add(new CoreConstants.Report(1100, "RPT0001", "Statement of Account", "Print statement of accounts.", "RPT0001.DLL"));
        Items.Add(new CoreConstants.Report(1101, "RPT0002", "Statement of Account - Roro", "Print RORO statement of accounts.", "RPT0002.DLL"));
        Items.Add(new CoreConstants.Report(1102, "RPT0003", "Schedule of Gross Revenue", "PPA Due - Schedule of Gross Revenue by date range", "RPT0003.DLL", UserInterfaces.Reports.RPT0003View));
        Items.Add(new CoreConstants.Report(1103, "RPT0004", "Statement of Account - Mooring/Unmooring", "Print Mooring/Unmooring statement of accounts.", "RPT0004.DLL"));
        Items.Add(new CoreConstants.Report(1104, "RPT0005", "Statement of Account - Porterage", "Print Porterage statement of accounts.", "RPT0005.DLL"));
        Items.Add(new CoreConstants.Report(1105, "RPT0006", "Summary of Gross Revenue", "PPA Due - Summary of Gross Revenue by date range", "RPT0006.DLL", UserInterfaces.Reports.RPT0006View));
        Items.Add(new CoreConstants.Report(1106, "RPT0007", "Arrastre / Stevedoring Summary", "PPA Due - Arrastre / Stevedoring Summary by date range", "RPT0007.DLL", UserInterfaces.Reports.RPT0007View));
        Items.Add(new CoreConstants.Report(1107, "RPT0008", "Summary of Monthly Gross Income Report", "PPA Due - Print Summary of Monthly Gross Income Report by date range", "RPT0008.DLL", UserInterfaces.Reports.RPT0008View));
        Items.Add(new CoreConstants.Report(1108, "RPT0009", "Summary of Gross Income Report", "PPA Due - Summary of Gross Income Report by date range", "RPT0009.DLL", UserInterfaces.Reports.RPT0009View));
        Items.Add(new CoreConstants.Report(1109, "RPT0010", "Roro Terminal Fee(s) - Reconciliation Report", "Strong Republic Nautical Highway(SRNH) Terminal Fee - Reconciliation Report", "RPT0010.DLL"));
        Items.Add(new CoreConstants.Report(1110, "RPT0011", "Statement of Acccount - SRNH Service Fee", "SRNH Service Fee Summary Report by date range", "RPT0011.DLL", UserInterfaces.Reports.RPT0011View));
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
