using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Data
{
  public interface IPageCriteria
  {
    int PageIndex { get; set; }

    int PageSize { get; set; }

    int TotalItemCount { get; set; }

    int TotalPage { get; set; }
  }
}
