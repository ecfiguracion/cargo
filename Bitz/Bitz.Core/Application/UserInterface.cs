using Bitz.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Application
{
  public class UserInterfaces
  {
    static UserInterfaces()
    {
      Bitz.Initialise();
      DashBoard.Initialise();
      Reports.Initialise();
      Cargo.Initialise();
      Payroll.Initialise();
    }

    #region Items

    private static List<CoreConstants.UserInterface> _Items = new List<CoreConstants.UserInterface>();
    private static List<CoreConstants.UserInterface> Items
    {
      get { return _Items; }
      set { _Items = value; }
    }

    #endregion

    // Id from 1100 to 1199
    #region Bitz

    public static class Bitz
    {
      public static CoreConstants.UserInterface ContactSelectDialog { get { return FindById(1100); } }
      public static CoreConstants.UserInterface ReportManager { get { return FindById(1101); } }

      public static void Initialise()
      {
        Items.Add(new CoreConstants.UserInterface(1100, "Select Entity", "Bitz.Views.Dialogs.SelectContactDialogView", Modules.Bitz, CoreConstants.PageType.Dialog));
        Items.Add(new CoreConstants.UserInterface(1101, "Report Manager", "Bitz.Views.Report.ReportManagerView", Modules.Bitz, CoreConstants.PageType.Page));
      }
    }

    #endregion

    // Id from 1200 to 1299
    #region DashBoard

    public static class DashBoard
    {
      public static CoreConstants.UserInterface CargoDashBoard { get { return FindById(1200); } }

      public static void Initialise()
      {
        Items.Add(new CoreConstants.UserInterface(1200, "Dashboard","Bitz.Dashboard.Views.CargoDashBoardView", Modules.DashBoard, CoreConstants.PageType.Page));
      }
    }

    #endregion

    // Id from 1300 to 1399
    #region Reports

    public static class Reports
    {
      public static CoreConstants.UserInterface RPT0003View { get { return FindById(1300); } }
      public static CoreConstants.UserInterface RPT0006View { get { return FindById(1301); } }

      public static void Initialise()
      {
        Items.Add(new CoreConstants.UserInterface(1300, "RPT-0003 Schedule of Gross Revenue", "Bitz.Reports.Views.RPT0003View", Modules.Reports, CoreConstants.PageType.Dialog));
        Items.Add(new CoreConstants.UserInterface(1301, "RPT-0006 Summary of Gross Revenue", "Bitz.Reports.Views.RPT0006View", Modules.Reports, CoreConstants.PageType.Dialog));
      }

    }

    #endregion

    // Id from 1400 to 1499
    #region Cargo

    public static class Cargo
    {
      public static CoreConstants.UserInterface BillingMain { get { return FindById(1400); } }

      public static CoreConstants.UserInterface ForeignBill { get { return FindById(1401); } }
      public static CoreConstants.UserInterface ForeignBills { get { return FindById(1402); } }

      public static CoreConstants.UserInterface DomesticBill { get { return FindById(1403); } }      
      public static CoreConstants.UserInterface DomesticBills { get { return FindById(1404); } }

      public static CoreConstants.UserInterface RoroBill { get { return FindById(1405); } }
      public static CoreConstants.UserInterface RoroBills { get { return FindById(1406); } }

      public static CoreConstants.UserInterface BillingInquiry { get { return FindById(1407); } }

      public static CoreConstants.UserInterface CargoSelectDialog { get { return FindById(1408); } }

      public static CoreConstants.UserInterface References { get { return FindById(1409); } }

      public static CoreConstants.UserInterface Consignee { get { return FindById(1410); } }
      public static CoreConstants.UserInterface Consignees { get { return FindById(1411); } }

      public static CoreConstants.UserInterface Vessel { get { return FindById(1412); } }
      public static CoreConstants.UserInterface Vessels { get { return FindById(1413); } }

      public static CoreConstants.UserInterface CargoItem { get { return FindById(1414); } }
      public static CoreConstants.UserInterface CargoItems { get { return FindById(1415); } }

      public static CoreConstants.UserInterface UnitOfMeasure { get { return FindById(1416); } }
      public static CoreConstants.UserInterface UnitOfMeasures { get { return FindById(1417); } }

      public static CoreConstants.UserInterface Payment { get { return FindById(1418); } }
      public static CoreConstants.UserInterface Payments { get { return FindById(1419); } }

      public static CoreConstants.UserInterface BankAccounts { get { return FindById(1420); } }

      public static CoreConstants.UserInterface WalkInBill { get { return FindById(1421); } }
      public static CoreConstants.UserInterface WalkInBills { get { return FindById(1422); } }

      public static CoreConstants.UserInterface BillSelectDialog { get { return FindById(1423); } }

      public static CoreConstants.UserInterface PaymentDetailsDialog { get { return FindById(1424); } }

      public static CoreConstants.UserInterface ConsigneeDialog { get { return FindById(1425); } }

      public static CoreConstants.UserInterface MooringBill { get { return FindById(1426); } }
      public static CoreConstants.UserInterface MooringBills { get { return FindById(1427); } }

      public static CoreConstants.UserInterface PorterageBill { get { return FindById(1428); } }
      public static CoreConstants.UserInterface PorterageBills { get { return FindById(1429); } }

      public static void Initialise()
      {
        Items.Add(new CoreConstants.UserInterface(1400, "Billings", "Bitz.Cargo.Views.BillingMainView", Modules.Cargo, CoreConstants.PageType.Page));
        
        Items.Add(new CoreConstants.UserInterface(1401, "Foreign","Foreign Billing", "Bitz.Cargo.Views.Billings.ForeignView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1402, "Foreign","Foreign Billings","Bitz.Cargo.Views.Billings.ForeignsView", Modules.Cargo, CoreConstants.PageType.ContentPage,Cargo.ForeignBill));

        Items.Add(new CoreConstants.UserInterface(1403, "Domestic","Domestic Billing", "Bitz.Cargo.Views.Billings.DomesticView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1404, "Domestic","Domestic Billings", "Bitz.Cargo.Views.Billings.DomesticsView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.DomesticBill));

        Items.Add(new CoreConstants.UserInterface(1405, "Roro","Roro Billing", "Bitz.Cargo.Views.Billings.RoroView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1406, "Roro","Roro Billings", "Bitz.Cargo.Views.Billings.RorosView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.RoroBill));

        Items.Add(new CoreConstants.UserInterface(1407, "Inquiry","Billing Inquiry", "Bitz.Cargo.Views.Billings.InquiryView", Modules.Cargo, CoreConstants.PageType.ContentPage));

        Items.Add(new CoreConstants.UserInterface(1408, "Select Cargo","Bitz.Cargo.Views.Dialogs.CargoSelectDialogView", Modules.Cargo, CoreConstants.PageType.Dialog));

        Items.Add(new CoreConstants.UserInterface(1409, "References", "Bitz.Cargo.Views.Settings.ReferencesView", Modules.Cargo, CoreConstants.PageType.Page));
        
        Items.Add(new CoreConstants.UserInterface(1410, "Consignee", "Bitz.Cargo.Views.Settings.ConsigneeView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1411, "Consignees", "Bitz.Cargo.Views.Settings.ConsigneesView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.Consignee));

        Items.Add(new CoreConstants.UserInterface(1412, "Vessel", "Bitz.Cargo.Views.Settings.VesselView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1413, "Vessels", "Bitz.Cargo.Views.Settings.VesselsView", Modules.Cargo, CoreConstants.PageType.ContentPage,Cargo.Vessel));

        Items.Add(new CoreConstants.UserInterface(1414, "Cargo", "Bitz.Cargo.Views.Settings.CargoView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1415, "Cargoes", "Bitz.Cargo.Views.Settings.CargosView", Modules.Cargo, CoreConstants.PageType.ContentPage,Cargo.CargoItem));

        Items.Add(new CoreConstants.UserInterface(1416, "Unit Of Measure", "Bitz.Cargo.Views.Settings.UnitOfMeasureView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1417, "Unit Of Measures", "Bitz.Cargo.Views.Settings.UnitOfMeasuresView", Modules.Cargo, CoreConstants.PageType.ContentPage,Cargo.UnitOfMeasure));

        Items.Add(new CoreConstants.UserInterface(1418, "Payment", "Bills Payment", "Bitz.Cargo.Views.Billings.PaymentView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1419, "Payment", "Bills Payment", "Bitz.Cargo.Views.Billings.PaymentsView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.Payment));

        Items.Add(new CoreConstants.UserInterface(1420, "Bank Accounts","Bitz.Cargo.Views.Settings.BankAccountsView", Modules.Cargo, CoreConstants.PageType.ContentPage));

        Items.Add(new CoreConstants.UserInterface(1421, "Walk-In", "Walk-In Billing", "Bitz.Cargo.Views.Billings.WalkInView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1422, "Walk-In", "Walk-In Billings", "Bitz.Cargo.Views.Billings.WalkInsView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.WalkInBill));

        Items.Add(new CoreConstants.UserInterface(1423, "Select SOA", "Bitz.Cargo.Views.Dialogs.BillSelectDialogView", Modules.Cargo, CoreConstants.PageType.Dialog));

        Items.Add(new CoreConstants.UserInterface(1424, "Payment Details", "Bitz.Cargo.Views.Dialogs.PaymentDetailsDialogView", Modules.Cargo, CoreConstants.PageType.Dialog));
        Items.Add(new CoreConstants.UserInterface(1425, "Consignee", "Bitz.Cargo.Views.Dialogs.ConsigneeDialogView", Modules.Cargo, CoreConstants.PageType.Dialog));

        Items.Add(new CoreConstants.UserInterface(1426, "Mooring", "Mooring/Unmooring", "Bitz.Cargo.Views.Billings.MooringView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1427, "Mooring", "Moorings/Unmoorings", "Bitz.Cargo.Views.Billings.MooringsView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.MooringBill));

        Items.Add(new CoreConstants.UserInterface(1428, "Porterage", "Porterage", "Bitz.Cargo.Views.Billings.PorterageView", Modules.Cargo, CoreConstants.PageType.ContentPage));
        Items.Add(new CoreConstants.UserInterface(1429, "Porterage", "Porterages", "Bitz.Cargo.Views.Billings.PorteragesView", Modules.Cargo, CoreConstants.PageType.ContentPage, Cargo.PorterageBill));
      }
    }

    #endregion

    // Id from 1500 to 1599
    #region Payroll

    public static class Payroll
    {
      public static void Initialise()
      {

      }
    }

    #endregion

    #region Utilities
    
    #region FindById

    public static CoreConstants.UserInterface FindById(int id) {
      CoreConstants.UserInterface userinterface = null;
      foreach (var item in Items) {
        if (item.Id == id) {
          userinterface = item;
          break;
        }
      }
      return userinterface;
    }

    #endregion

    #region FindByName

    public static CoreConstants.UserInterface FindByName(string name) {
      CoreConstants.UserInterface userinterface = null;
      foreach (var item in Items) {
        if (item.Name == name) {
          userinterface = item;
          break;
        }
      }
      return userinterface;
    }

    #endregion

    #region GetParent

    public static CoreConstants.UserInterface GetParent(CoreConstants.UserInterface childUI)
    {
      CoreConstants.UserInterface userinterface = null;
      foreach (var item in Items.FindAll(x => x.LinkUI != null))
      {
        if (item.LinkUI.Id == childUI.Id)
        {
          userinterface = item;
          break;
        }
      }
      return userinterface;
    }

    #endregion

    #endregion
  }
}
