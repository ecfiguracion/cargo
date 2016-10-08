using Bitz.Business.Contacts.Infos;
using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Bitz.Core.ViewModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class RoroVM : PageViewModelBase<Roro>
  {
    #region Initialise

    public RoroVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

      this.CommandAddItemRate = new DelegateCommand<object>(CommandAddItemRateExecute);
      this.CommandRemoveItemRate = new DelegateCommand<object>(CommandRemoveItemRateExecute);

      this.CommandAddItemRateOther = new DelegateCommand<object>(CommandAddItemRateOtherExecute);
      this.CommandRemoveItemRateOther = new DelegateCommand<object>(CommandRemoveItemRateOtherExecute);
    }

    public override void Initialise(int? id)
    {
      this.LoadLookupReferences((isloaded) =>
      {
        if (isloaded)
        {
          base.Initialise(id);
          LoadConfiguredItemRates();
        }
      });
    }

    #endregion

    #region Internal Events

    #endregion

    #region Properties

    //Consignees
    private BaseContactInfos _Consignees;
    public BaseContactInfos Consignees
    {
      get { return _Consignees; }
      set
      {
        _Consignees = value;
        OnPropertyChanged("Consignees");
      }
    }

    //Vessels
    private BaseContactInfos _Vessels;
    public BaseContactInfos Vessels
    {
      get { return _Vessels; }
      set
      {
        _Vessels = value;
        OnPropertyChanged("Vessels");
      }
    }

    //Cargos
    private ItemInfos _Cargos;
    public ItemInfos Cargos
    {
      get { return _Cargos; }
      set
      {
        _Cargos = value;
        OnPropertyChanged("Cargos");
      }
    }

    //UOM
    private UnitOfMeasureInfos _Units;
    public UnitOfMeasureInfos Units
    {
      get { return _Units; }
      set
      {
        _Units = value;
        OnPropertyChanged("Units");
      }
    }

    //ConfiguredItemRates
    private ItemRateInfos _ConfiguredItemRates;
    public ItemRateInfos ConfiguredItemRates
    {
      get { return _ConfiguredItemRates; }
      set
      {
        _ConfiguredItemRates = value;
        OnPropertyChanged("ConfiguredItemRates");
      }
    }

    //SelectedConfiguredItemRate
    private ItemRateInfo _SelectedConfiguredItemRate;
    public ItemRateInfo SelectedConfiguredItemRate
    {
      get { return _SelectedConfiguredItemRate; }
      set
      {
        _SelectedConfiguredItemRate = value;
        OnPropertyChanged("SelectedConfiguredItemRate");
        OnPropertyChanged("CanRemoveItemRate");
        OnPropertyChanged("CanAddItemRate");
      }
    }

    public bool CanAddItemRate
    {
      get { return this.SelectedConfiguredItemRate != null; }
    }

    public bool CanRemoveItemRate
    {
      get { return this.SelectedItemRate != null; }
    }

    public bool CanRemoveItemRateOther
    {
      get { return this.SelectedItemRateOther != null; }
    }

    //SelectedItemRates
    private ItemRateInfos _SelectedItemRates;
    public ItemRateInfos SelectedItemRates
    {
      get { return _SelectedItemRates; }
      set
      {
        _SelectedItemRates = value;
        OnPropertyChanged("SelectedItemRates");
      }
    }

    //SelectedItemRate
    private BillingItemRate _SelectedItemRate;
    public BillingItemRate SelectedItemRate
    {
      get { return _SelectedItemRate; }
      set
      {
        _SelectedItemRate = value;
        OnPropertyChanged("SelectedItemRate");
        OnPropertyChanged("CanRemoveItemRate");
        OnPropertyChanged("CanAddItemRate");
      }
    }

    //SelectedItemRateOther
    private BillingItemRateOther _SelectedItemRateOther;
    public BillingItemRateOther SelectedItemRateOther
    {
      get { return _SelectedItemRateOther; }
      set
      {
        _SelectedItemRateOther = value;
        OnPropertyChanged("SelectedItemRateOther");
        OnPropertyChanged("CanRemoveItemRateOther");
      }
    }

    public Visibility ConfiguredRatesVisibility
    {
      get
      {
        if (this.Model == null || this.Model.Cargo == null || this.ConfiguredItemRates == null)
          return Visibility.Hidden;

        return this.ConfiguredItemRates.Any() ? Visibility.Visible : Visibility.Hidden;
      }
    }

    public Visibility SelectedRatesVisibility
    {
      get
      {
        if (this.Model == null || this.Model.RoroHandlingRates == null)
          return Visibility.Hidden;

        return this.Model.RoroHandlingRates.Any() ? Visibility.Visible : Visibility.Hidden;
      }
    }

    public List<CoreConstants.IdValue> HandlingChargeType
    {
      get { return Bitz.Cargo.Business.Constants.CargoConstants.CargoHandlingChargeTypes.Items; }
    }
    #endregion

    #region Commands

    public ICommand CommandAddItemRate
    {
      get;
      private set;
    }

    public void CommandAddItemRateExecute(object parameter)
    {
      if (this.Model.RoroHandlingRates == null)
        this.Model.RoroHandlingRates = new BillingItemRates();

      var itemrate = this.Model.RoroHandlingRates.AddNew();
      itemrate.ItemRate = this.SelectedConfiguredItemRate.Id;
      if (this.Model.ItemCountHandling != null)
        itemrate.Computation1 = Math.Round(this.Model.ItemCountHandling.Value, 2);
      if (this.SelectedConfiguredItemRate.ConstantValue > 0)
        itemrate.Computation2 = this.SelectedConfiguredItemRate.ConstantValue;

      itemrate.Computation3 = this.SelectedConfiguredItemRate.ItemRate;
      LoadConfiguredItemRates();

    }

    public ICommand CommandRemoveItemRate
    {
      get;
      private set;
    }

    public void CommandRemoveItemRateExecute(object parameter)
    {
      if (SelectedItemRate != null)
      {
        this.Model.RoroHandlingRates.Remove(SelectedItemRate);
        LoadConfiguredItemRates();
      }
    }

    public ICommand CommandAddItemRateOther
    {
      get;
      private set;
    }

    public void CommandAddItemRateOtherExecute(object parameter)
    {
      if (this.Model.RoroHandlingRateOthers == null)
        this.Model.RoroHandlingRateOthers = new BillingItemRateOthers();

      var itemrateother = this.Model.RoroHandlingRateOthers.AddNew();

    }

    public ICommand CommandRemoveItemRateOther
    {
      get;
      private set;
    }

    public void CommandRemoveItemRateOtherExecute(object parameter)
    {
      if (SelectedItemRateOther != null)
      {
        this.Model.RoroHandlingRateOthers.Remove(SelectedItemRateOther);
      }
    }

    public override void CommandCancelExecute(object parameter)
    {
      base.DoCancel();
      //DoRefresh("Get", this);
    }

    public override void CommandPrintExecute(object parameter)
    {
      if (!this.Model.IsNew)
      {
        ReportHelper.Print(Reports.Cargo.StatementOfAccount, this.Model.Id);
      }
    }
    #endregion

    #region Methods

    #region LoadLookupReferences

    private void LoadLookupReferences(Action<bool> resultCallback)
    {
      var datasourcestotal = 4;
      var datasourcescount = 0;

      if (this.Consignees == null)
      {
        BaseContactInfos.Get(new BaseContactInfos.Criteria() { ContactType = BitzConstants.ContactTypes.Consignee.Id }, (o, e) =>
        {
          if (e.Error != null) throw e.Error;

          this.Consignees = e.Object;

        });
      }
      ++datasourcescount;
      if (datasourcescount == datasourcestotal)
        resultCallback(true);

      if (this.Vessels == null)
      {
        BaseContactInfos.Get(new BaseContactInfos.Criteria() { ContactType = BitzConstants.ContactTypes.Vessel.Id }, (o, e) =>
        {
          if (e.Error != null) throw e.Error;

          this.Vessels = e.Object;

        });
      }
      ++datasourcescount;
      if (datasourcescount == datasourcestotal)
        resultCallback(true);

      if (this.Cargos == null)
      {
        ItemInfos.Get(new ItemInfos.Criteria(), (o, e) =>
        {
          if (e.Error != null) throw e.Error;

          this.Cargos = e.Object;

        });
      }
      ++datasourcescount;
      if (datasourcescount == datasourcestotal)
        resultCallback(true);

      if (this.Units == null)
      {
        UnitOfMeasureInfos.Get(new UnitOfMeasureInfos.Criteria(), (o, e) =>
        {
          if (e.Error != null) throw e.Error;

          this.Units = e.Object;

        });
      }
      ++datasourcescount;
      if (datasourcescount == datasourcestotal)
        resultCallback(true);


    }

    #endregion

    private void LoadConfiguredItemRates()
    {
      if (this.Model == null || this.Model.Cargo == null)
        return;

      var excludeIds = string.Empty;
      if (this.Model.RoroHandlingRates != null)
        excludeIds = string.Join(",", this.Model.RoroHandlingRates.Select(r => r.ItemRate).ToArray());

      ItemRateInfos.Get(new ItemRateInfos.Criteria() { ItemId = this.Model.Cargo.Value, ExcludeStringIds = excludeIds }, (o, e) =>
      {
        if (e.Error != null) throw e.Error;
        this.ConfiguredItemRates = e.Object;

        OnPropertyChanged("ConfiguredRatesVisibility");
        OnPropertyChanged("SelectedRatesVisibility");
      });
      OnPropertyChanged("CanPrint");
    }

    #endregion
  }
}
