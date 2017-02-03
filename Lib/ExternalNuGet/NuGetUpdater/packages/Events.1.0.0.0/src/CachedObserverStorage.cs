using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class CachedObserverStorage : IObserverStorage
    {
        private IObserverStorage _actualStorage;
        private ObjectCache _cache;

        public CachedObserverStorage(ObjectCache cache, IObserverStorage actualStorage)
        {
            _cache = cache;
            _actualStorage = actualStorage;
        }

        public IEnumerable<IEventObserver<TEvent>> ListObservers<TEvent>()
        {
            var key = string.Format("CachedObserverStorage:{0}", typeof(TEvent).AssemblyQualifiedName);

            if (!_cache.Contains(key))
                _cache[key] = _actualStorage.ListObservers<TEvent>();

            return (IEnumerable<IEventObserver<TEvent>>)_cache[key];
        }
    }
}
