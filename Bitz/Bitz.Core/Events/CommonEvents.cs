using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace Bitz.Core.Events
{
  public class CommonEvents
  {
    public class DialogResultEvent :  PubSubEvent<object> { }
  }
}
