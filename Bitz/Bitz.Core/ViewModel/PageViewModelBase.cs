using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Csla.Xaml;
using FirstFloor.ModernUI.Windows.Controls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Bitz.Core.ViewModel
{
  public abstract class PageViewModelBase<TModel> : ViewModelBase<TModel>, IViewModelBase
  {
        #region Constructor
    public PageViewModelBase()
            : base()
        {
            this.CommandSave = new DelegateCommand<object>(CommandSaveExecute);
            this.CommandUndo = new DelegateCommand<object>(CommandUndoExecute);
            this.CommandPrint = new DelegateCommand<object>(CommandPrintExecute);
            this.CommandBack = new DelegateCommand<object>(CommandBackExecute);
            this.CommandCancel = new DelegateCommand<object>(CommandCancelExecute);
        }
        #endregion

        #region Initialise

        public virtual void Initialise()
        {
           
        }

        public virtual void Initialise(int? id)
        {
            if (id == null)
            {
                BeginRefresh("New");
            }
            else
            {
                BeginRefresh("Get", new object[] { id });
            }
            this.IsReadOnly = false;
        }

        #endregion

        #region Events 

        #endregion

        #region Properties

        #region Can Properties

        #region CanSave
        private bool _CanSave = false;

        public virtual bool CanSave
        {
            get 
            {
              if (IsReadOnly) return false;
              return base.CanSave;
            }
        }
        #endregion

        #region CanCancel
        private bool _CanCancel = false;

        public virtual bool CanCancel
        {
            get 
            {
              if (IsReadOnly) return false;
              return base.CanCancel;
            }
        }
        #endregion

        #region CanPrint
        public virtual bool CanPrint
        {
          get
          {
            var isdirty = base.IsDirty;
            return !isdirty;
          }
        }
        #endregion

        #region CanCancelDocument
        public virtual bool CanCancelDocument
        {
          get
          {
            if (IsReadOnly) return false;
            var isdirty = base.IsDirty;
            return !isdirty;
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
                OnPropertyChanged("CanSave");
                OnPropertyChanged("CanCancel");
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

        #region View

        public UserControl View { get; set; }

        #endregion

        #endregion

        #endregion

        #region Command

        #region CommandSave
        public ICommand CommandSave
        {
            get;
            private set;
        }

        public virtual void CommandSaveExecute(object parameter)
        {
            BeginSave();
        }
        #endregion

        #region CommandUndo
        public ICommand CommandUndo
        {
            get;
            private set;
        }

        public virtual void CommandUndoExecute(object parameter)
        {
            base.DoCancel();
        }
        #endregion

        #region CommandPrint
        public ICommand CommandPrint
        {
            get;
            private set;
        }

        public virtual void CommandPrintExecute(object parameter)
        {
        }
        #endregion

        #region CommandBack
        public ICommand CommandBack
        {
            get;
            private set;
        }

        public virtual void CommandBackExecute(object parameter)
        {
          var isproceed = true;
          if (base.IsDirty && !this.IsReadOnly) { 
            var result = ModernDialog.ShowMessage("Uncommitted changes will be lost, are you sure you want to proceed?",
              "Close", MessageBoxButton.YesNo,AppCache.MainWindow);
            isproceed = result == MessageBoxResult.Yes;
          }

          //Is this UI link to parent, if yes, go back to it
          if (isproceed)
          {
            var parentUI = UserInterfaces.GetParent(this.UserInterface);
            if (parentUI != null)
            {
              NavigationManager.Show(parentUI);
            }
          }
        }
        #endregion

        #region CommandCancel
        public ICommand CommandCancel
        {
          get;
          private set;
        }

        public virtual void CommandCancelExecute(object parameter)
        {
        }
        #endregion

        #endregion

        #region Methods
        #endregion
  }
}
