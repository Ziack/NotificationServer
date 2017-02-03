using NotificationServer.Contract;
using NotificationServer.Core.Runtime.Job;
using Rhino.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Utilities;

namespace NotificationServer.Windows.Application.Jobs
{
    public class NotificationConsumer : ConsumerOf<Notification>
    {
        private readonly IDictionary<NotificationType, INotificationJob> _jobs;

        private readonly IServiceBus _serviceBus;

        public NotificationConsumer(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
            _jobs = JobFactory.Create();
        }

        public void Consume(Notification message)
        {
            var job = _jobs[message.Type];
            job.ParseJobMessage(message.ToBinary());
            job.Execute();
        }
    }
}
