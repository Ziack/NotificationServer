using NotificationServer.Core.Runtime.Services;
using System.Diagnostics;

namespace NotificationServer.Windows.Application.Services
{
    public class TraceService : ITraceService
    {
        public void TraceError(string error)
        {
            Trace.TraceError(error);
        }

        public void TraceInformation(string info)
        {
            Trace.TraceInformation(info);
        }

        public void TraceWarning(string warning)
        {
            Trace.TraceWarning(warning);
        }
    }
}
