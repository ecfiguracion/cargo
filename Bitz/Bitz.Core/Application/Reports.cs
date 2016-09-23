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
      public static CoreConstants.Report StatementOfAccount { get { return FindById(1100); } }
      public static void Initialise()
      {
        Items.Add(new CoreConstants.Report(1100, "BL0001", "Statement of Account", "Print statement of accounts.", "SOA.DLL"));
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

    #endregion
  }
}
