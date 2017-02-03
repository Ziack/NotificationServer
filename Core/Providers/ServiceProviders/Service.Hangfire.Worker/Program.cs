using Hangfire;
using Hangfire.Pro.Redis;
using NotificationServer.Config;
using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Service.Hangfire.Worker
{
    class Program
    {
        private static BackgroundJobServer _server;
        static void Main(string[] args)
        {
            HostFactory.Run(factory =>
            {
                var notificationServiceConfig = (ConfigurationManager.GetSection("notificationService") as NotificationServiceConfigSection);
                var jobStorage = notificationServiceConfig.NotificationsScheduler.Storage.Build<JobStorage>();

                GlobalConfiguration.Configuration.UseStorage(jobStorage);
                GlobalJobFilters.Filters.Add(new JobContext());

                Insight.Database.SqlInsightDbProvider.RegisterProvider();

                factory.Service<BackgroundJobServer>(service =>
                {
                    service.ConstructUsing(name =>
                    {
                        Int32 workerCount;

                        if(!Int32.TryParse(ConfigurationManager.AppSettings["Hangfire.WorkerCount"], out workerCount))
                            workerCount = Environment.ProcessorCount * 5;

                        var rawEnabledQueues = ConfigurationManager.AppSettings["Hangfire.Queues"];
                        var enabledQueues = rawEnabledQueues.Split('|').Select(t => (SchedulingPriority)Enum.Parse(typeof(SchedulingPriority), t));                        

                        var options = new BackgroundJobServerOptions
                        {
                            WorkerCount = workerCount,
                            Queues = enabledQueues.Select(t => $"{ t }".ToLower()).ToArray()
                        };

                        _server = new BackgroundJobServer(options);

                        return _server;
                    });

                    service.WhenStarted(tc =>
                    {
                        _server.Start();
                    });

                    service.WhenStopped(tc => _server.Dispose());
                });

                factory.DependsOnEventLog();
                factory.RunAsLocalSystem();

                //var description = ConfigurationManager.AppSettings["HangfireServer.Description"];
                //var displayName = ConfigurationManager.AppSettings["HangfireServer.DisplayName"];
                //var serviceName = ConfigurationManager.AppSettings["HangfireServer.ServiceName"];

                //factory.SetDescription(description);
                //factory.SetDisplayName(displayName);
                //factory.SetServiceName(serviceName);
            });
        }
    }
}
