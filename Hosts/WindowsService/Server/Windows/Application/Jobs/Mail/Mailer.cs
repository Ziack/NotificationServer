using System;
using System.Linq;
using System.Collections.Generic;
using NotificationServer.Core;
using NotificationServer.Core.Senders;
using System.Text;
using System.Net.Mail;
using NotificationServer.Core.Runtime.Services;
using NotificationServer.Core.Runtime.Job;
using AttachmentCollection = NotificationServer.Core.AttachmentCollection;
using NotificationServer.Contract;
using NotificationServer.Contract.Utilities;
using System.IO;

namespace NotificationServer.Windows.Application.Jobs.Mail
{
    public class Mailer : NotifierBase, INotifier
    {
        private readonly ITraceService _traceService;

        public Mailer(ITemplateEngineService templateService, ISender mailSender = null, Encoding defaultMessageEncoding = null, ITraceService traceService = null)
            : base(templateService: templateService, mailSender: mailSender, defaultMessageEncoding: defaultMessageEncoding)
        {
            _traceService = traceService;
        }

        public NotificationResult Run(String templateName, String from, String subject, IList<String> to, IList<String> CC, IList<String> BCC, IList<String> replyTo, IDictionary<String, Object> properties, AttachmentCollection attachment)
        {
            var notification = new Notification
            {
                From = from,
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
            notification.Attachments.AddAll(
                attachment.Select(kv => new Contract.Attachment(new MemoryStream(kv.Value), kv.Key))
            );

            return Notify(notification);
        }

        protected override void OnNotificationSending(NotificationSendingContext context)
        {
            _traceService.TraceInformation(String.Format(SN.ON_MAIL_SENDING, context.Notification.To.ToString(), context.Notification.Subject));
        }

        protected override void OnNotificationSent(NotificationSentContext mail)
        {
            _traceService.TraceInformation(String.Format(SN.ON_MAIL_SENT, mail.Notification.To.ToString(), mail.Notification.Subject));
        }
    }
}
