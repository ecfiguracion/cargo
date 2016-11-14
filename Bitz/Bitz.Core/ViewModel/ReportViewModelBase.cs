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
  public abstract class ReportViewModelBase<TModel> : ViewModelBase<TModel>,IViewModelBase
  {
        #region Constructor
    public ReportViewModelBase()
            : base()
        {
          this.CommandRun = new DelegateCommand<object>(CommandRunExecute);
          this.CommandCancel = new DelegateCommand<object>(CommandCancelExecute);
        }
        #endregion

        #region Initialise

        public virtual void Initialise()
        {

        }

        public virtual void Initialise(CoreConstants.Report report)
        {
          this.Report = report;
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

        #region View
        public ModernDialog View { get; set; }
        #endregion

        #region Parameters

        public string Parameters { get; set; }

        #endregion

        #region Report

        private CoreConstants.Report _Report;

        public CoreConstants.Report Report
        {
          get { return _Report; }
          set
          {
            _Report = value;
            OnPropertyChanged("Report");
          }
        }

        #endregion

        #endregion

        #region Command

        #region CommandRun
        public ICommand CommandRun
        {
          get;
          private set;
        }

        public virtual void CommandRunExecute(object parameter)
        {
          ReportHelper.Print(this.Report.Assembly, parameter.ToString());
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
