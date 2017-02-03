using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using NotificationServer.Core;
using NotificationServer.Azure.Application;
using System;
using NotificationServer.Azure.Application.Jobs;
using NotificationServer.Azure.Application.Services;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Diagnostics;
using log4net;

namespace NotificationServer.Azure
{
    public class WorkerRole : RoleEntryPoint
    {
        private volatile bool onStopCalled = false;
        private volatile bool returnedFromRunMethod = false;
        private QueueMessageProcessor _messageProcessor;

        private static readonly ILog _log = LogManager.GetLogger(Globals.LOG4NET_WORKERROLE_SOURCE);

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount;
            ConfigureDiagnostics();

            var connectionString = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBus.ConnectionString");
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);
            
            var jobs = JobFactory.Create();
            
            _messageProcessor = new QueueMessageProcessor(
                new SleepService(),
                new JobQueue(storageAccount),
                new TraceService(),
                new QueueMessageParser(jobs));

            return base.OnStart();
        }

        public override void OnStop()
        {
            onStopCalled = true;
            while (returnedFromRunMethod == false)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public override void Run()
        {
            CloudQueueMessage msg = null;
            Trace.TraceInformation("BackgroundWorker start of Run()");
            while (true)
            {
                if (onStopCalled == true)
                {
                    Trace.TraceInformation("OnStop() called BackgroundWorker");
                    returnedFromRunMethod = true;
                    return;
                }

                _messageProcessor.Run(msg);
            }
        }

        private void ConfigureDiagnostics()
        {
            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.ConfigurationChangePollInterval = TimeSpan.FromMinutes(1d);
            config.Logs = new BasicLogsBufferConfiguration();
            config.Logs.BufferQuotaInMB = 500;
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", config);
        }
    }
}
