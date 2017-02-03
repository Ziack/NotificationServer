using NotificationServer.Core;
using NotificationServer.Core.Senders;
using NotificationServer.Service.Commands;
using NotificationServer.Service.Entities;
using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;

namespace NotificationServer.Service
{
    [Serializable]
    public class NotificationRunner : NotifierBase
    {
        private NotificationSpec _spec;

        private IEnumerable<INotificationInterceptor> _interceptors;

        public INotificationsRepository Notifications { get; set; }


        public NotificationSpec NotificationSpec
        {
            get { return _spec; }
            set
            {
                _spec = value;
                Sender = _spec.BuildSender();
            }
        }


        public NotificationRunner()
        {

        }

        public NotificationRunner(
            NotificationSpec spec,
            INotificationsRepository notifications,
            ITemplateEngineService templateService
        ) : base(
            templateService,
            spec.BuildSender(),
            defaultMessageEncoding: spec.Encoding
        )
        {
            Notifications = notifications;
            _spec = spec;
            _interceptors = new INotificationInterceptor[] { };
        }

        public void Run()
        {

            var notification = Notifications.Get(_spec.MessageId).GetNotificationFor(_spec.ServiceName);

            try
            {
                _interceptors = _spec.BuildNotificationInterceptors();

                Notify(notification).Deliver(_spec.Options);

                Notifications.ReportNotificationStatus(ReportNotificationStatusCommand.NotificationHasBeenSent(_spec.MessageId, _spec.ServiceName, notification.PartitionKey));
            }
            catch (Exception ex)
            {
                Notifications.ReportNotificationStatus(ReportNotificationStatusCommand.NotificationHasError(_spec.MessageId, notification.PartitionKey, ex, _spec.ServiceName));
                throw;
            }
        }

        protected override void OnNotificationSending(NotificationSendingContext context)
        {
            Array.ForEach(_interceptors.ToArray(), i => i.OnNotificationSending(ref context));
        }

        protected override void OnNotificationSent(NotificationSentContext mail)
        {
            Array.ForEach(_interceptors.ToArray(), i => i.OnNotificationSent(mail));
        }
    }
}
