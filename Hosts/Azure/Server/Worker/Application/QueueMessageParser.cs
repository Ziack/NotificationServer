using Microsoft.WindowsAzure.Storage.Queue;
using NotificationServer.Contract;
using NotificationServer.Core.Runtime.Job;
using NotificationServer.Core.Runtime.Queue;
using System.Collections.Generic;

namespace NotificationServer.Azure.Application
{
    public class QueueMessageParser : IQueueMessageParser
    {
        private readonly IDictionary<NotificationType, INotificationJob> _jobs;

        public QueueMessageParser(IDictionary<NotificationType, INotificationJob> jobs)
        {
            _jobs = jobs;
        }

        public void Parse(CloudQueueMessage message)
        {
            var notification = message.FromMessage<Notification>();
            var job = _jobs[notification.Type];
            job.ParseJobMessage(message.AsBytes);
            job.Execute();
        }
    }
}
