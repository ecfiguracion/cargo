using Bitz.Cargo.Business.Billing;
using Bitz.Cargo.Business.CargoReferences.Infos;
using Bitz.Cargo.Business.Contacts;
using Bitz.Cargo.Business.Contacts.Infos;
using Bitz.Cargo.Business.Items;
using Bitz.Cargo.Business.Items.Infos;
using Bitz.Core.Application;
using Bitz.Core.Constants;
using Bitz.Core.Shell;
using Bitz.Core.Utilities;
using Bitz.Core.ViewModel;
using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bitz.Cargo.ViewModels.Billings
{
  public class RoroVM : PageViewModelBase<Roro>
  {
    #region Initialise

    public override void Initialise(int? id)
    {
      this.LoadLookupReferences((isloaded) =>
      {
        if (isloaded)
        {
          base.Initialise(id);
        }
      });
    }

    #endregion

    #region Properties

    //Consignees
    private ContactInfos _Consignees;
    public ContactInfos Consignees
    {
      get { return _Consignees; }
      set
      {
        _Consignees = value;
        OnPropertyChanged("Consignees");
      }
    }

    //Vessels
    private ContactInfos _Vessels;
    public ContactInfos Vessels
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

    #endregion

    #region Commands

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
        ContactInfos.Get(new ContactInfos.Criteria() { ContactType = 2 }, (o, e) =>
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
        ContactInfos.Get(new ContactInfos.Criteria() { ContactType = 3 }, (o, e) =>
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

    #endregion
  }
}
