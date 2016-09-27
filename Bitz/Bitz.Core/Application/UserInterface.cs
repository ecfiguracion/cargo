﻿using Bitz.Core.Constants;
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
      public static void Initialise()
      {

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
      public static void Initialise()
      {

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
