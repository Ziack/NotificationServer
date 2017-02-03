using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class SequentialObserverInvoker : IObserversInvoker
    {
        public void Invoke<TEvent>(IEnumerable<IEventObserver<TEvent>> observers, TEvent evnt)
        {
            foreach (var observer in observers)
                observer.On(evnt);
        }
    }
}
