using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitz.Core.Events
{
  public class EventAggregator
  {
    private static Prism.Events.EventAggregator eventAggregator;

    public static TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
    {
      if (eventAggregator == null)
        eventAggregator = new Prism.Events.EventAggregator();

      return eventAggregator.GetEvent<TEventType>();
    }

    public static void Dispose()
    {
      eventAggregator = null;
    }
  }
}
