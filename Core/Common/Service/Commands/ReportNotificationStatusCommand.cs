using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Commands
{
    public class ReportNotificationStatusCommand
    {
        public Guid NotificationId { get; set; }

        public Guid PartitionKey { get; set; }

        public string ServiceName { get; set; }
        public string Status { get; set; }

        public string Description { get; set; }

        public Exception Error { get; set; }

        public static ReportNotificationStatusCommand NotificationHasBeenCreated(Guid notificationId, Guid partitionKey)
        {
            return NotificationHasBeen(notificationId, partitionKey, "Created");
        }

        public static ReportNotificationStatusCommand NotificationHasBeenReceived(Guid notificationId, Guid partitionKey)
        {
            return NotificationHasBeen(notificationId, partitionKey, "Received");
        }

        public static ReportNotificationStatusCommand NotificationHasBeenSent(Guid notificationId, string service, Guid partitionKey)
        {
            return NotificationHasBeen(notificationId, partitionKey, "Sent", service);
        }

        public static ReportNotificationStatusCommand NotificationHasBeen(Guid notificationId, Guid partitionKey, string status, string service = null)
        {
            return new ReportNotificationStatusCommand
            {
                NotificationId = notificationId,
                Status = status,
                ServiceName = service,
                PartitionKey = partitionKey
            };
        }

        public static ReportNotificationStatusCommand NotificationHasError(Guid notificationId,Guid partitionKey, Exception error, string service = null)
        {
            return new ReportNotificationStatusCommand
            {
                NotificationId = notificationId,
                Status = "Error",
                Error = error,
                ServiceName = service,
                PartitionKey = partitionKey
            };
        }
    }

    public static class ReportNotificationStatusCommandExtensions
    {
        public static ReportNotificationStatusCommand WithDescription(this ReportNotificationStatusCommand self, string description)
        {
            self.Description = description;
            return self;
        }

    }
}
