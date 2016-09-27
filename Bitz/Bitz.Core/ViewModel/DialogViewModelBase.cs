using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Events;
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
  public abstract class DialogViewModelBase<TModel> : ViewModelBase<TModel>,IViewModelBase
  {
        #region Constructor
    public DialogViewModelBase()
            : base()
        {
          this.CommandSelect = new DelegateCommand<object>(CommandSelectExecute);
          this.CommandRefresh = new DelegateCommand<object>(CommandRefreshExecute);
          this.CommandCancel = new DelegateCommand<object>(CommandCancelExecute);
        }
        #endregion

        #region Initialise

        public virtual void Initialise()
        {
           
        }

        #endregion

        #region Events 

        #endregion

        #region Properties

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

        #region CanSelect

        public virtual bool CanSelect
        {
          get
          {
            return SelectedItem != null;
          }
        }

        #endregion

        #region View
        public ModernDialog View { get; set; }
        #endregion

        #region Criteria

        public virtual object Criteria { get; set; }

        #endregion

        #region SelectedItem

        private object _SelectedItem;

        public object SelectedItem
        {
          get { return _SelectedItem; }
          set
          {
            _SelectedItem = value;
            OnPropertyChanged("CanSelect");
          }
        }

        #endregion

        #endregion

        #region Command

        #region CommandSelect
        public ICommand CommandSelect
        {
          get;
          private set;
        }

        public virtual void CommandSelectExecute(object parameter)
        {
          if (SelectedItem != null)
          {
            EventAggregator.GetEvent<CommonEvents.DialogResultEvent>().Publish(this.SelectedItem);
            View.Close();
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

        #region CommandCancel
        public ICommand CommandCancel
        {
            get;
            private set;
        }

        public virtual void CommandCancelExecute(object parameter)
        {
          if (this.View != null)
          {
            View.Close();
          }
        }
        #endregion

        #endregion

        #region Methods

        public virtual void Refresh() {

        }

        #endregion
  }
}
