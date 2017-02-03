using NotificationServer.Windows.Application.Jobs;
using NotificationServer.Windows.Backend;
using Rhino.ServiceBus.Hosting;
using Rhino.ServiceBus.Msmq;
using System.Configuration;
using Topshelf;

namespace NotificationServer.Windows
{
    class Program
    {
        private static DefaultHost _app;

        static void Main(string[] args)
        {
            HostFactory.Run(factory =>
            {
                factory.Service<DefaultHost>(service =>
                {
                    service.ConstructUsing(name =>
                    {
                        _app = new DefaultHost();
                        return _app;
                    });

                    service.WhenStarted(tc =>
                    {
                        PrepareQueues.Prepare("msmq://localhost/Notification.Backend", QueueType.Standard);
                        _app.Start<BackendBootStrapper>();                        
                    });

                    service.WhenStopped(tc => _app.Dispose());
                });

                factory.DependsOnEventLog();
                factory.RunAsLocalSystem();

                var description = ConfigurationManager.AppSettings["NotificationServer.Description"];
                var displayName = ConfigurationManager.AppSettings["NotificationServer.DisplayName"];
                var serviceName = ConfigurationManager.AppSettings["NotificationServer.ServiceName"];

                factory.SetDescription(description);
                factory.SetDisplayName(displayName);
                factory.SetServiceName(serviceName);
            });            
            
        }
    }
}
