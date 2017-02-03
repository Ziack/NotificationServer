using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class EventEmitter
    {
        private readonly IObserversInvoker _invoker;
        private readonly IObserverStorage _storage;
        
        public EventEmitter(IObserverStorage storage)
            : this(storage, new SequentialObserverInvoker())
        {

        }

        public EventEmitter(IObserverStorage storage, IObserversInvoker invoker)
        {
            _storage = storage;
            _invoker = invoker;
        }

        public void Emit<TEvent>(TEvent evnt)
        {
            var observers = _storage.ListObservers<TEvent>();

            _invoker.Invoke(observers, evnt);
        }
    }
}
