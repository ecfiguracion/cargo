using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla.Xaml;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using System.Windows.Input;
using Bitz.Core.Data;
using Prism.Commands;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using Bitz.Core.Commands;
using Bitz.Core.Application;

namespace Bitz.Core.ViewModel
{
  public abstract class GridViewModelBase<TModel> : ViewModelBase<TModel>,IViewModelBase
  {
    public GridViewModelBase()
      : base()
    {
      this.CommandNew = new DelegateCommand<object>(CommandNewExecute);
      this.CommandOpen = new DelegateCommand<object>(CommandOpenExecute);
      this.CommandRemove = new DelegateCommand<object>(CommandRemoveExecute);
      this.CommandRefresh = new DelegateCommand<object>(CommandRefreshExecute);

      this.CommandMoveNext = new DelegateCommand<object>(CommandMoveNextExecute);
      this.CommandMovePrevious = new DelegateCommand<object>(CommandMovePreviousExecute);
      this.CommandMoveLast = new DelegateCommand<object>(CommandMoveLastExecute);
      this.CommandMoveFirst = new DelegateCommand<object>(CommandMoveFirstExecute);
    }

    #region Initialise

    public virtual void Initialise()
    {

    }

    #endregion

    #region Events

    protected override void OnModelChanged(TModel oldValue, TModel newValue)
    {
      base.OnModelChanged(oldValue, newValue);
      OnPropertyChanged("PageInfo");
    }

    #endregion

    #region Properties

    #region Can Properties

    #region CanAdd

    public virtual bool CanAdd
    {
      get
      {
        return !IsReadOnly;
      }
    }

    #endregion

    #region CanOpen

    public virtual bool CanOpen
    {
      get
      {
        return SelectedItem != null;
      }
    }

    #endregion

    #region CanRemove

    public virtual bool CanRemove
    {
      get
      {
        return !IsReadOnly && this.SelectedItem != null;
      }
    }

    #endregion

    #endregion

    #region VMBase Properties

    #region IsReadOnly
    private bool _IsReadOnly;

    public bool IsReadOnly
    {
      get { return _IsReadOnly; }
      set
      {
        _IsReadOnly = value;
        OnPropertyChanged("CanAdd");
        OnPropertyChanged("CanRemove");
      }
    }
    #endregion

    #region IsBusy
    private bool _IsBusy;

    public bool IsBusy
    {
      get { return _IsBusy; }
      set
      {
        _IsBusy = value;
      }
    }
    #endregion

    #region UserInterface

    private CoreConstants.UserInterface _UserInterface;

    public CoreConstants.UserInterface UserInterface
    {
      get { return _UserInterface; }
      set
      {
        _UserInterface = value;
        OnPropertyChanged("UserInterface");
      }
    }

    #endregion

    #endregion

    #region SelectedItem

    private object _SelectedItem;

    public object SelectedItem
    {
      get { return _SelectedItem; }
      set
      {
        _SelectedItem = value;
        OnPropertyChanged("CanOpen");
        OnPropertyChanged("CanRemove");
      }
    }

    #endregion

    #region Criteria

    public virtual IPageCriteria Criteria { get; set; }

    #endregion

    #region PageInfo

    public string PageInfo
    {
      get
      {
        if (base.Model != null)
          return string.Format("Page {0} of {1}", Criteria.PageIndex, Criteria.TotalPage);
        else
          return "Page 1 of 1";
      }
    }

    #endregion

    #endregion

    #region Commands

    #region CommandNew
    public ICommand CommandNew
    {
      get;
      private set;
    }

    public virtual void CommandNewExecute(object parameter)
    {
      if (this.UserInterface.LinkUI != null)
      {
        NavigationManager.Show(this.UserInterface.LinkUI, new object[] { null });
      }
    }
    #endregion

    #region CommandOpen
    public ICommand CommandOpen
    {
      get;
      private set;
    }

    public virtual void CommandOpenExecute(object parameter)
    {
      if (this.SelectedItem != null && this.UserInterface.LinkUI != null)
      {
        var IdProperty = this.SelectedItem.GetType().GetProperty("Id");
        if (IdProperty != null)
        {
          var id = IdProperty.GetValue(this.SelectedItem, null);
          NavigationManager.Show(this.UserInterface.LinkUI, new object[] { id });
        }
      }
    }
    #endregion

    #region CommandRemove
    public ICommand CommandRemove
    {
      get;
      private set;
    }

    public virtual void CommandRemoveExecute(object parameter)
    {
      if (this.SelectedItem != null)
      {
        var tablename = string.Empty;
        var keycolumn = string.Empty;
        var keycolumnvalue = 0;

        var IdProperty = this.SelectedItem.GetType().GetProperty("Id");
        if (IdProperty != null)
        {
          keycolumnvalue = (int)IdProperty.GetValue(this.SelectedItem, null);
        }

        System.Attribute[] attributeinfos = System.Attribute.GetCustomAttributes(SelectedItem.GetType());
        foreach (var attribute in attributeinfos)
        {
          if (attribute is TableInfoAttribute)
          {
            var tableAttribute = (TableInfoAttribute)attribute;
            tablename = tableAttribute.TableName;
            keycolumn = tableAttribute.KeyColumn;
            break;
          }
        }

        if (tablename.Length > 0 && keycolumn.Length > 0 && keycolumnvalue > 0)
        {
          var result = ModernDialog.ShowMessage("This will permanently remove the record from the database. \n\nAre you sure you want to proceed?",
            "Remove", MessageBoxButton.YesNo,AppCache.MainWindow);
          if (result == MessageBoxResult.Yes)
          {
            CommandRemoveRow.Execute(tablename, keycolumn, keycolumnvalue, (o, e) =>
            {
              if (e.Error != null) ;
              this.Refresh();
            });          
          }
        }
      }
    }
    #endregion

    #region CommandRefresh
    public ICommand CommandRefresh
    {
      get;
      private set;
    }

    public virtual void CommandRefreshExecute(object parameter)
    {
      this.Refresh();
    }
    #endregion

    #region CommandMoveNext
    public ICommand CommandMoveNext
    {
      get;
      private set;
    }

    public virtual void CommandMoveNextExecute(object parameter)
    {
      if (this.Criteria.PageIndex < this.Criteria.TotalPage)
      {
        this.Criteria.PageIndex += 1;
        this.Refresh();
      }
    }
    #endregion

    #region CommandMovePrevious
    public ICommand CommandMovePrevious
    {
      get;
      private set;
    }

    public virtual void CommandMovePreviousExecute(object parameter)
    {
      if (this.Criteria.PageIndex > 1)
      {
        this.Criteria.PageIndex -= 1;
        this.Refresh();
      }
    }
    #endregion

    #region CommandMoveLast
    public ICommand CommandMoveLast
    {
      get;
      private set;
    }

    public virtual void CommandMoveLastExecute(object parameter)
    {
      if (this.Criteria.PageIndex != this.Criteria.TotalPage)
      {
        this.Criteria.PageIndex = this.Criteria.TotalPage;
        this.Refresh();
      }
    }
    #endregion

    #region CommandMoveFirst
    public ICommand CommandMoveFirst
    {
      get;
      private set;
    }

    public virtual void CommandMoveFirstExecute(object parameter)
    {
      if (this.Criteria.PageIndex != 1)
      {
        this.Criteria.PageIndex = 1;
        this.Refresh();
      }
    }
    #endregion

    #endregion

    #region Methods

    public void Refresh()
    {
      this.BeginRefresh("Get", new object[] { this.Criteria });
    }

    #endregion
  }
}
