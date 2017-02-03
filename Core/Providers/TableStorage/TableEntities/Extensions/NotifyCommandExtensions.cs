using NotificationServer.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities.Extensions
{
    public static class NotifyCommandExtensions
    {
        public static NotificationTableEntity ToNotificationTableEntity(this NotifyCommand command)
        {
            var notificationEntity = new NotificationTableEntity
            {
                RowKey = Convert.ToString(command.Id),
                ApplicationName = command.ApplicationName,

            };

            return notificationEntity;
        }
    }
}
