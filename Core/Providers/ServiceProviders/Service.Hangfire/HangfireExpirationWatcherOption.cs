using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Hangfire
{
    internal class HangfireExpirationWatcherOption
    {
        public TimeSpan SleepTimeout { get; set; }

        public TimeSpan FetchedLockTimeout { get; set; }
    }
}
