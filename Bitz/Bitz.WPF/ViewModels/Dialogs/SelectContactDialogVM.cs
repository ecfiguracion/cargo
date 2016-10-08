using Bitz.Business.Contacts.Infos;
using Bitz.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.ViewModels.Dialogs
{
  public class SelectContactDialogVM : DialogViewModelBase<BaseContactInfos>
  {

    public void Initialise(int type)
    {
      this.Criteria.ContactType = type;
      this.Refresh();
    }

    #region Properties

    public BaseContactInfos.Criteria _Criteria = new BaseContactInfos.Criteria();
    public BaseContactInfos.Criteria Criteria
    {
      get { return _Criteria; }
      set
      {
        _Criteria = value;
        OnPropertyChanged("Criteria");
      }
    }
    #endregion

    #region Methods

    public override void Refresh()
    {
      BaseContactInfos.Get(this.Criteria, (o, e) =>
      {
        if (e.Error != null) throw e.Error;
        this.Model = e.Object;
      });
    }

    #endregion
  }
}
