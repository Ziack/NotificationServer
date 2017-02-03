using NotificationServer.Contract;
using NotificationServer.Contract.Commands;
using NotificationServer.Service.Commands;
using System;

namespace NotificationServer.Service.Repositories
{
    public interface INotificationsRepository
    {
        Guid Save(NotifyCommand notification);

        Guid AddToBatch(Guid notificationId, Guid batchId);

        NotifyCommand Get(Guid messageId);

        void ReportNotificationStatus(ReportNotificationStatusCommand status);
    }
}