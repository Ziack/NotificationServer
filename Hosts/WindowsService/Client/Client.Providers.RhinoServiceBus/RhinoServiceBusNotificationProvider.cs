using NotificationServer.Client.Configuration;
using NotificationServer.Client.Providers.RhinoServiceBus.Utilities;
using NotificationServer.Contract;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Hosting;
using Rhino.ServiceBus.Msmq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Commands;

namespace NotificationServer.Client.Providers.RhinoServiceBus
{
    public class RhinoServiceBusNotificationProvider : NotificationProvider
    {
        public override Guid Send(NotifyCommand notification)
        {
            throw new NotImplementedException("This method will be implemented as soon as the Azure Storage and the Windows Service interfaces of notificator pass to the new schema");
        }

        [Obsolete]
        public override void Send(Notification notification)
        {
            var host = new DefaultHost();
            host.BusConfiguration((cfg) =>
                {
                    cfg.Bus(endpoint: "msmq://localhost/Notification.Client", name: "client");
                    cfg.Threads(threadCount: 1);
                    cfg.Retries(numberOfRetries: 5);
                    cfg.Receive(messageName: typeof(Notification).FullName, endpoint: this.ConnectionStringName);

                    return cfg;
                });

            host.Start<ClientBootStrapper>();
            var bus = host.Bus as IServiceBus;

            bus.Send(notification);
        }

        [Obsolete]
        public override Guid Provisioning(ProvisioningCommand provisioning)
        {
            throw new NotImplementedException();
        }
    }
}
