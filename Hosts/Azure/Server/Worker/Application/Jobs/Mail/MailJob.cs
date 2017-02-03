using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NotificationServer.Core.Utilities;
using NotificationServer.Core.Runtime.Job;
using System.Collections;
using NotificationServer.Core;
using NotificationServer.Core.Utilities;

namespace NotificationServer.Azure.Application.Jobs.Mail
{
    public class MailJob : INotificationJob
    {
        public Notification Notification { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private readonly INotifier _mailer;

        public MailJob(INotifier mailer)
        {
            _mailer = mailer;
        }

        public void ParseJobMessage(Byte[] message)
        {
            Notification = message.Deserialize<Notification>();
        }

        public void Execute()
        {
            var result = _mailer.Run(
                     templateName: Notification.TemplateName,
                     from: Notification.From,
                     subject: Notification.Subject,
                     to: Notification.To,
                     CC: Notification.CC,
                     BCC: Notification.BCC,
                     replyTo: Notification.ReplyTo,
                     properties: Notification.Properties.ToDictionary(kvp => (String)kvp.Key, kvp => kvp.Value),
                     attachment: Notification.Attachments != null ? Notification.Attachments.ToDictionary(kvp => kvp.Name, kvp => kvp.ContentStream.ToByteArray()).ToAttachmentCollection() : new AttachmentCollection()
                 );

            result.DeliverAsync();
        }
    }
}
