using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Constants
{
  public class CoreConstants
  {
    #region IdValue

    public class IdValue
    {
      public int Id { get; set; }
      public string Value { get; set; }

      public IdValue(int id, string value)
      {
        this.Id = id;
        this.Value = value;
      }
    }

    #endregion

    #region Module

    public class Module
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string AssemblyName { get; set; }

      public Module(int id, string name, string assemblyname)
      {
        this.Id = id;
        this.Name = name;
        this.AssemblyName = assemblyname;
      }

    }

    #endregion

    #region PageType

    public enum PageType
    {
      Page = 1,
      Dialog = 2,
      ContentPage = 3
    }

    #endregion

    #region Region

    public enum Region
    {
      MainRegion = 1,
      ContentRegion = 3
    }

    #endregion

    #region UserInterface

    public class UserInterface
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Display { get; set; }
      public string Page { get; set; }
      public CoreConstants.Module Module { get; set; }
      public CoreConstants.PageType PageType { get; set; }
      public CoreConstants.UserInterface LinkUI { get; set; }
      public CoreConstants.Region Region { get; set; }

      public string Assembly
      {
        get
        {
          return string.Format("{0},{1}, Version=...,Culture=neutral,PublicKeyToken=null", this.Page,
              this.Module.AssemblyName);
        }
      }

      public string ViewModel
      {
        get
        {
          return Page.Replace("View", "VM");
        }
      }

      public Uri Uri
      {
        get
        {
          var module = "/" + this.Module.AssemblyName;
          var view = this.Page.Replace(this.Module.AssemblyName + ".",string.Empty);
          view = "component/" + view.Replace(".","/") + ".xaml";
          return new Uri(module + ";" + view,UriKind.Relative);
        }
      }

      public UserInterface()
      {

      }

      public UserInterface(int id, string name, string page, CoreConstants.Module module, CoreConstants.PageType pagetype)
      {
        this.Initialise(id, name, name, page, module, pagetype, null);
      }

      public UserInterface(int id, string name,string display, string page, CoreConstants.Module module, CoreConstants.PageType pagetype)
      {
        this.Initialise(id, name,display, page, module, pagetype, null);
      }

      public UserInterface(int id, string name, string page, CoreConstants.Module module, CoreConstants.PageType pagetype, CoreConstants.UserInterface linkUI)
      {
        this.Initialise(id, name, name, page, module, pagetype, linkUI);
      }

      public UserInterface(int id, string name,string display, string page, CoreConstants.Module module, CoreConstants.PageType pagetype, CoreConstants.UserInterface linkUI)
      {
        this.Initialise(id, name,display, page, module, pagetype, linkUI);
      }

      private void Initialise(int id, string name,string display, string page, CoreConstants.Module module, CoreConstants.PageType pagetype, CoreConstants.UserInterface linkUI)
      {
        this.Id = id;
        this.Name = name;
        this.Display = display;
        this.Page = page;
        this.Module = module;
        this.PageType = pagetype;
        this.LinkUI = linkUI;

        if (pagetype == CoreConstants.PageType.Page)
          this.Region = CoreConstants.Region.MainRegion;
        else if (pagetype == CoreConstants.PageType.ContentPage)
          this.Region = CoreConstants.Region.ContentRegion;
      }
    }

    #endregion

    #region Report

    public class Report
    {
      public int Id { get; set; }
      public string Code { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
      public string Assembly { get; set; }
      public UserInterface UserInterface { get; set; }

      public Report(int id, string code, string name, string description, string assembly)
      {
        this.Initialise(id, code, name, description, assembly, null);
      }

      public Report(int id, string code, string name, string description,string assembly,UserInterface userinterface)
      {
        this.Initialise(id, code, name, description, assembly, userinterface);
      }

      private void Initialise(int id, string code, string name, string description, string assembly, UserInterface userinterface)
      {
        this.Id = id;
        this.Code = code;
        this.Name = name;
        this.Description = description;
        this.Assembly = assembly;
        this.UserInterface = userinterface;
      }
    }

    #endregion
  }
}
