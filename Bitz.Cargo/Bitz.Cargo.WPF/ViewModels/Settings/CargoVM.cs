using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.Billing.Infos;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Constants;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Events;
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

namespace Bitz.Cargo.ViewModels.Settings
{
  public class CargoVM : PageViewModelBase<Item>
  {
    #region Initialise

    public CargoVM()
    {
    }

    public override void Initialise(int? id)
    {
      this.CommandAddRate = new DelegateCommand<object>(CommandAddRateExecute);
      this.CommandRemoveRate = new DelegateCommand<object>(CommandRemoveRateExecute);
      this.CommandAddUomConversion = new DelegateCommand<object>(CommandAddUomConversionExecute);
      this.CommandRemoveUomConversion = new DelegateCommand<object>(CommandRemoveUomConversionExecute);

      UnitOfMeasureInfos.Get(new UnitOfMeasureInfos.Criteria(), (oo, ee) =>
      {
        if (ee.Error != null) throw ee.Error;
        this.Units = ee.Object;
        base.Initialise(id);
      });

    }

    #endregion

    #region Internal Events

    #endregion

    #region Properties

    #region CanPrint
    public override bool CanPrint
    {
      get
      {
        return false;
      }
    }
    #endregion

    #region Units
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
    #endregion

    #region CargoCharges
    public List<CoreConstants.IdValue> CargoCharges
    {
      get { return CargoConstants.CargoHandlingCharges.Items; }
    }
    #endregion

    #region SelectedItemRate
    public ItemRate SelectedItemRate { get; set; }
    #endregion

    #region SelectedItemUomConversion
    public ItemUomConversion SelectedItemUomConversion { get; set; }
    #endregion

    #endregion

    #region Commands

    #region CommandAddRate
    public ICommand CommandAddRate
    {
      get;
      private set;
    }

    public void CommandAddRateExecute(object parameter)
    {
      if (this.Model.ItemUnitRates == null)
        this.Model.ItemUnitRates = new ItemRates();

      this.Model.ItemUnitRates.AddNew();

    }

    #endregion

    #region CommandRemoveRate

    public ICommand CommandRemoveRate
    {
      get;
      private set;
    }
    public void CommandRemoveRateExecute(object parameter)
    {
      if (SelectedItemRate != null)
      {
        this.Model.ItemUnitRates.Remove(SelectedItemRate);
      }
    }

    #endregion

    #region CommandAddUomConversion
    public ICommand CommandAddUomConversion
    {
      get;
      private set;
    }

    public void CommandAddUomConversionExecute(object parameter)
    {
      if (this.Model.ItemUomConversions == null)
        this.Model.ItemUomConversions = new ItemUomConversions();

      this.Model.ItemUomConversions.AddNew();

    }
    #endregion

    #region CommandRemoveUomConversion
    public ICommand CommandRemoveUomConversion
    {
      get;
      private set;
    }

    public void CommandRemoveUomConversionExecute(object parameter)
    {
      if (SelectedItemUomConversion != null)
      {
        this.Model.ItemUomConversions.Remove(SelectedItemUomConversion);
      }
    }
    #endregion

    #endregion

    #region Methods

    #endregion
  }
}
