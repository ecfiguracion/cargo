using Bitz.Core.Utilities;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Data
{
  [Serializable]
  public abstract class PageCriteriaBase<T> : BusinessBase<T>,IPageCriteria
  where T : PageCriteriaBase<T>
  {
    static PageCriteriaBase() { }

    protected PageCriteriaBase()
    {
      this.PageSize = ConfigHelper.GetPageSize();
      this.PageIndex = 1;
    }

    #region PageIndex
    private static PropertyInfo<int> _PageIndex = RegisterProperty<int>(c => c.PageIndex);

    public int PageIndex
    {
      get { return GetProperty(_PageIndex); }
      set { SetProperty(_PageIndex, value); }
    }
    #endregion

    #region PageSize
    private static PropertyInfo<int> _PageSize = RegisterProperty<int>(c => c.PageSize);
    public int PageSize
    {
      get { return GetProperty(_PageSize); }
      set { SetProperty(_PageSize, value); }
    }
    #endregion

    #region TotalItemCount
    private static PropertyInfo<int> _TotalItemCount = RegisterProperty<int>(c => c.TotalItemCount);
    public int TotalItemCount
    {
      get { return GetProperty(_TotalItemCount); }
      set 
      { 
        SetProperty(_TotalItemCount, value);

        double totalpage = (double)this.TotalItemCount / (double)this.PageSize;
        double decimalpart = totalpage - Math.Truncate(totalpage);

        int actualpage = (int)totalpage;
        if (decimalpart > 0) actualpage += 1;

        this.TotalPage = actualpage;
      }
    }
    #endregion

    #region TotalPage
    private static PropertyInfo<int> _TotalPage = RegisterProperty<int>(c => c.TotalPage);
    public int TotalPage
    {
      get { return GetProperty(_TotalPage); }
      set { SetProperty(_TotalPage, value); }
    }
    #endregion

  }
}
