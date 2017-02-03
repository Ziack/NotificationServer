using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public interface IObserversInvoker
    {
        void Invoke<TEvent>(IEnumerable<IEventObserver<TEvent>> observers, TEvent evnt);
    }
}
