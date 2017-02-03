using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public interface IEventObserver { }

    public interface IEventObserver<TEvent> : IEventObserver
    {
        void On(TEvent evnt);
    }
}
