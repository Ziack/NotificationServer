using NotificationServer.Contract.Commands;
using NotificationServer.Core;
using NotificationServer.Service.Commands;
using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationServer.Service
{
    public class NotificationServerService
    {
        private IConfigurationsRepository _configurations;
        private INotificationsRepository _notifications;
        private INotificationsScheduler _notificationsScheduler;
        private ITemplateEngineService _templateEngine;

        public NotificationServerService(
            INotificationsRepository notifications,
            IConfigurationsRepository configurations,
            INotificationsScheduler notificationsScheduler,
            ITemplateEngineService templateEngine
        )
        {
            _notifications = notifications;
            _configurations = configurations;
            _notificationsScheduler = notificationsScheduler;
            _templateEngine = templateEngine;
        }

        public Guid Send(NotifyCommand command)
        {

            var notificationId = _notifications.Save(command);

            //TODO: Eliminar?
            _notifications.ReportNotificationStatus(
                ReportNotificationStatusCommand.NotificationHasBeenCreated(notificationId, command.Id)
                    .WithDescription
                    (
                        String.Format("Notification added for services: [{0}]", command.Destinations.Count == 0 ? "NONE!" : string.Join(", ", command.Destinations.Select(t => t.Service)))
                    )
                );

            IList<String> DestinationsTo = new List<String>();

            foreach (var item in command.Destinations)
            {
                foreach (var To in item.To)
                {
                    DestinationsTo.Add(To);
                }
            }

            _notifications.ReportNotificationStatus(
                ReportNotificationStatusCommand.NotificationHasBeenCreated(notificationId, command.Id)
                    .WithDescription
                    (
                        String.Format("Notification Destinations To: [{0}]", DestinationsTo.Any() ? string.Join(", ", DestinationsTo.Select(t => t)) : "NONE!")
                    )
                );

            _notifications.ReportNotificationStatus(
                ReportNotificationStatusCommand.NotificationHasBeenCreated(notificationId, command.Id)
                    .WithDescription
                    (
                        String.Format("Notification TemplatesNames To: [{0}]", command.Destinations.Count == 0 ? "NONE!" : string.Join(", ", command.Destinations.Select(t => t.TemplateName)))
                    )
                );

            if (command.Response != null)
                //TODO: Emilinar?
                _notifications.ReportNotificationStatus(
                    ReportNotificationStatusCommand.NotificationHasBeenReceived(notificationId, command.Id)
                    .WithDescription
                    (
                        String.Format("Notification response: [{0}]", command.Response)
                    )
                );

            if (command.urlSender != null)
                //TODO: Emilinar?
                _notifications.ReportNotificationStatus(
                    ReportNotificationStatusCommand.NotificationHasBeenCreated(notificationId, command.Id)
                    .WithDescription
                    (
                        String.Format("Notification Url Sender: [{0}]", command.urlSender)
                    )
                );

            var forServices = new List<string>();

            try
            {

                var notificationSpecs = _configurations.GetNotificationSpecsFor(command);

                //TODO: Eliminar?
                _notifications.ReportNotificationStatus(
                    ReportNotificationStatusCommand.NotificationHasBeenCreated(notificationId, command.Id)
                    .WithDescription
                    (
                        String.Format("Services found: [{0}]", notificationSpecs.Count() == 0 ? "NONE!" : string.Join(", ", notificationSpecs.Select(t => t.ServiceName)))
                    )
                    );

                foreach (var spec in notificationSpecs)
                {
                    //if (!command.Destinations.Any(c => c.Service == spec.ServiceName))
                    //    continue;

                    spec.MessageId = notificationId;

                    spec.PartitionKey = command.Id;

                    _notificationsScheduler.Add(new NotificationRunner(spec, _notifications, _templateEngine));

                    forServices.Add(spec.ServiceName);
                }

                _notifications.ReportNotificationStatus(
                    ReportNotificationStatusCommand.NotificationHasBeen(notificationId, command.Id, "Scheduled")
                    .WithDescription(
                        string.Format("Notification added for services: [{0}]", forServices.Count == 0 ? "NONE!" : string.Join(", ", forServices))
                    )
                );
            }
            catch (Exception ex)
            {
                _notifications.ReportNotificationStatus(ReportNotificationStatusCommand.NotificationHasError(notificationId, command.Id, ex));
                throw;
            }

            return notificationId;
        }

        public async Task<Guid> SendAsync(NotifyCommand command)
        {
            var result = Guid.Empty;
            await Task.Run(() => result = Send(command));
            return result;
        }
    }
}
