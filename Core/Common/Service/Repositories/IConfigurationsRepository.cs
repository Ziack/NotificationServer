using NotificationServer.Contract;
using NotificationServer.Service.Entities;
using System.Collections.Generic;
using NotificationServer.Contract.Commands;

namespace NotificationServer.Service.Repositories
{
    public interface IConfigurationsRepository
    {
        IEnumerable<NotificationSpec> GetNotificationSpecsFor(NotifyCommand notification);
    }
}