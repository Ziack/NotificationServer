using log4net;
using NotificationServer.Core;
using NotificationServer.Core.Runtime.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Azure.Application.Services
{
    public class TraceService : ITraceService
    {
        private static readonly ILog _log = LogManager.GetLogger(Globals.LOG4NET_WORKFLOW_SERVER_API_SOURCE);

        public void TraceError(string error)
        {
            _log.Error(error);
        }

        public void TraceInformation(string info)
        {
            _log.Error(info);
        }

        public void TraceWarning(string warning)
        {
            _log.Error(warning);
        }
    }
}
