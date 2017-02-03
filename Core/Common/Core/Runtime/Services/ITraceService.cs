using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Runtime.Services
{
    public interface ITraceService
    {
        void TraceError(string error);

        void TraceInformation(string info);

        void TraceWarning(string warning);
    }
}
