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
  public class PaymentVM : PageViewModelBase<Foreign>
  {
    #region Initialise

    public PaymentVM()
    {
      if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv") return;

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
          return Visibility.Hidden;

        return this.ConfiguredItemRates.Any() ? Visibility.Visible : Visibility.Hidden;
      }
    }

    public Visibility SelectedRatesVisibility
    {
      get
      {
          return Visibility.Hidden;
      }
    }

    public List<CoreConstants.IdValue> HandlingChargeType
    {
      get { return Bitz.Cargo.Business.Constants.CargoConstants.CargoHandlingChargeTypes.Items; }
    }
    #endregion

    #region Commands

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
        return;

      var excludeIds = string.Empty;
      //if (this.Model.ForeignHandlingRates != null)
      //  excludeIds = string.Join(",", this.Model.ForeignHandlingRates.Select(r => r.ItemRate).ToArray());

      //ItemRateInfos.Get(new ItemRateInfos.Criteria() { ItemId = this.Model.Cargo.Value, ExcludeStringIds = excludeIds }, (o, e) =>
      ItemRateInfos.Get(new ItemRateInfos.Criteria() {}, (o, e) =>
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
