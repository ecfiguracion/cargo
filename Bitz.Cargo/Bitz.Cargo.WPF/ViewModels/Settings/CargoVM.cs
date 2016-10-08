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
      this.CommandAddUomConversion = new DelegateCommand<object>(CommandAddUomConversionExecute);
      this.CommandRemoveUomConversion = new DelegateCommand<object>(CommandRemoveUomConversionExecute);
      this.CommandAddRate = new DelegateCommand<object>(CommandAddRateExecute);
      this.CommandRemoveRate = new DelegateCommand<object>(CommandRemoveRateExecute);


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

    #region SelectedItemUomConversion
    public ItemUomConversion SelectedItemUomConversion { get; set; }
    #endregion

    #region SelectedItemUomRate
    public ItemUomRate SelectedItemUomRate { get; set; }
    #endregion

    #endregion

    #region Commands

    #region CommandAddUomConversion
    public ICommand CommandAddUomConversion
    {
      get;
      private set;
    }

    public void CommandAddUomConversionExecute(object parameter)
    {
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

    #region CommandAddRate
    public ICommand CommandAddRate
    {
      get;
      private set;
    }

    public void CommandAddRateExecute(object parameter)
    {
      this.Model.ItemUomRates.AddNew();
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
      if (SelectedItemUomRate != null)
      {
        this.Model.ItemUomRates.Remove(SelectedItemUomRate);
      }
    }
    #endregion

    #endregion

    #region Methods

    #endregion
  }
}
