using Newtonsoft.Json.Linq;
using NotificationServer.Config;
using NotificationServer.Contract;
using NotificationServer.Contract.Commands;
using NotificationServer.Core;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NotificationServer.Interceptors.CoreInterceptors
{
    /// <summary>
    /// Response to be used and generates a new messages that triggers the whole workflow
    /// </summary>
    public class ReplyInterceptor : INotificationInterceptor
    {
        public void OnNotificationSending(ref NotificationSendingContext context) { }

        public void OnNotificationSent(NotificationSentContext notification)
        {
            if (!notification.Parameters.ContainsKey("ReplyToNotificationCommand"))
                return;

            var replyToNotificationCommand = (notification.Parameters["ReplyToNotificationCommand"] as JObject).ToObject<NotifyCommand>();
            replyToNotificationCommand.Id = notification.Notification.PartitionKey;
            replyToNotificationCommand.Response = notification.Response == null ? null : notification.Response.ToString();
            replyToNotificationCommand.urlSender = notification.HostSender;

            var notificationService = (ConfigurationManager.GetSection("notificationService") as NotificationServiceConfigSection).Build();

            replyToNotificationCommand.Properties = EnforceNotificationCommandOnlyHasOneLevelOfReplydata(
                replyToNotificationCommand.Properties
            );

            replyToNotificationCommand.Properties.Add(new NotificationProperty("Reply.OriginalNotificationData", notification.Notification.Properties));

            replyToNotificationCommand.Properties.Add(new NotificationProperty("Reply.OriginalNotificationResponse", notification.Response));

            notificationService.Send(replyToNotificationCommand);
        }

        private static IList<NotificationProperty> EnforceNotificationCommandOnlyHasOneLevelOfReplydata(IList<NotificationProperty> properties)
        {
            return properties
                .Where(p => p.Key != "Reply.OriginalNotificationData" && p.Key != "Reply.OriginalNotificationResponse")
                .ToList();
        }
    }
}
