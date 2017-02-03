using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class MemoryObserverStorage : IObserverStorage
    {
        private readonly IList<IEventObserver> _observers = new List<IEventObserver>();

        public MemoryObserverStorage(params IEventObserver[] observers)
        {
            foreach (var observer in observers)
                Add(observer);
        }

        public void Add(IEventObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            if (_observers.Contains(observer))
                return;

            _observers.Add(observer);
        }

        public void Remove(IEventObserver observer)
        {
            _observers.Remove(observer);
        }


        public IEnumerable<IEventObserver<TEvent>> ListObservers<TEvent>()
        {
            return _observers.Where(
                e => typeof(IEventObserver<TEvent>).IsAssignableFrom(e.GetType())
            ).Cast<IEventObserver<TEvent>>();
        }
    }
}
