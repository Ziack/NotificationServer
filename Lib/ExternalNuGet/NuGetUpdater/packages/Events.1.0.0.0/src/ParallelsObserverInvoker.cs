using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class ParallelsObserverInvoker : IObserversInvoker
    {
        private static readonly ParallelOptions _default = new ParallelOptions
        {
            MaxDegreeOfParallelism = 4
        };

        public ParallelOptions Options { get; set; }

        public ParallelsObserverInvoker()
            : this(_default)
        {

        }

        public ParallelsObserverInvoker(ParallelOptions parallelOptions)
        {
            Options = parallelOptions;
        }

        public void Invoke<TEvent>(IEnumerable<IEventObserver<TEvent>> observers, TEvent evnt)
        {
            Parallel.ForEach(observers, Options ?? _default, o => o.On(evnt));
        }
    }
}
