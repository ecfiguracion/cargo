using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Cargo.Business.Settings;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Events;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Bitz.Core.ViewModel;
using FirstFloor.ModernUI.Windows.Controls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bitz.Cargo.ViewModels.Settings
{
  public class BankAccountsVM : PageViewModelBase<BankAccount>
  {
    #region Initialise

    public BankAccountsVM()
    {
      this.CommandAddBank = new DelegateCommand<object>(CommandAddBankExecute);
      this.CommandRemoveBank = new DelegateCommand<object>(CommandRemoveBankExecute);

      BeginRefresh("Get");
    }

    #endregion

    #region Internal Events

    #endregion

    #region Properties

    public BankAccountDetail SelectedBankAccount { get; set; }

    #endregion

    #region Commands

    #region CommandAddBank
    public ICommand CommandAddBank
    {
      get;
      private set;
    }

    public void CommandAddBankExecute(object parameter)
    {
      this.Model.BankAccountDetails.AddNew();
    }
    #endregion

    #region CommandRemoveBank
    public ICommand CommandRemoveBank
    {
      get;
      private set;
    }

    public void CommandRemoveBankExecute(object parameter)
    {
      if (SelectedBankAccount != null)
      {
        var result = NavigationManager.ShowMessage("Remove","Are you sure you want to delete the selected record?",MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
          this.Model.BankAccountDetails.Remove(this.SelectedBankAccount);
        }

      }
    }
    #endregion

    #endregion

    #region Methods

    #endregion
  }
}
