using System;
using System.Collections.Generic;
using NotificationServer.Core;
using NotificationServer.Core.Senders;
using System.Text;
using NotificationServer.Core.Runtime.Services;
using NotificationServer.Core.Runtime.Job;
using AttachmentCollection = NotificationServer.Core.AttachmentCollection;
using NotificationServer.Contract;
using NotificationServer.Contract.Utilities;
using System.Linq;
using System.IO;

namespace NotificationServer.Azure.Application.Jobs.Mail
{
    public class Mailer : NotifierBase, INotifier
    {
        private readonly string _defaultFrom;

        private readonly ITraceService _traceService;

        private readonly Action<NotificationSendingContext> _onEmailSending;

        private readonly Action<Notification> _onEmailSent;

        public Mailer(ITemplateEngineService templateService, ISender sender = null, string defaultFrom = null, Encoding defaultMessageEncoding = null, ITraceService traceService = null, Action<NotificationSendingContext> onEmailSending = null, Action<Notification> onEmailSent = null)
            : base(templateService: templateService, mailSender: sender, defaultMessageEncoding: defaultMessageEncoding)
        {
            _defaultFrom = defaultFrom;
            _traceService = traceService;
            _onEmailSending = onEmailSending;
            _onEmailSent = onEmailSent;
        }

        public NotificationResult Run(string templateName, string from, string subject, IList<string> to, IList<string> CC, IList<string> BCC, IList<string> replyTo, IDictionary<string, Object> properties, AttachmentCollection attachment)
        {

            var notification = new Notification
            {
                From = _defaultFrom ?? from,
                Subject = subject,
                TemplateName = templateName
            };

            notification.To.AddAll(to);
            notification.CC.AddAll(CC);
            notification.BCC.AddAll(BCC);
            notification.ReplyTo.AddAll(replyTo);
            notification.Properties.AddAll(
                properties.Select(kv => new NotificationProperty(kv.Key, kv.Value))
            );

            if (attachment != null)
                notification.Attachments.AddAll(
                    attachment.Select(kv => new Attachment(new MemoryStream(kv.Value), kv.Key))
                );

            return Notify(notification);
        }

        protected override void OnNotificationSending(NotificationSendingContext context)
        {
            _traceService.TraceInformation(string.Format(SN.ON_MAIL_SENDING, context.Notification.To.ToString(), context.Notification.Subject));

            if (_onEmailSending != null)
                _onEmailSending(obj: context);
        }

        protected override void OnNotificationSent(NotificationSentContext mail)
        {
            _traceService.TraceInformation(string.Format(SN.ON_MAIL_SENT, mail.Notification.To.ToString(), mail.Notification.Subject));

            if (_onEmailSent != null)
                _onEmailSent(obj: mail.Notification);
        }
    }
}
