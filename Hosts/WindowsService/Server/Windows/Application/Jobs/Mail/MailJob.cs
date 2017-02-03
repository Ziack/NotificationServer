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


namespace NotificationServer.Windows.Application.Jobs.Mail
{
    public class MailJob : INotificationJob
    {
        /// <summary>
        /// 
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// A string representation of who this mail should be from.  Could be
        /// your name and email address or just an email address by itself.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The subject line of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// A collection of addresses this email should be sent to.
        /// </summary>
        public IList<string> To { get; private set; }

        /// <summary>
        /// A collection of addresses that should be CC'ed.
        /// </summary>
        public IList<string> CC { get; private set; }

        /// <summary>
        /// A collection of addresses that should be BCC'ed.
        /// </summary>
        public IList<string> BCC { get; private set; }

        /// <summary>
        /// A collection of addresses that should be listed in Reply-To header.
        /// </summary>
        public IList<string> ReplyTo { get; private set; }

        /// <summary>
        /// A collection of properties that should be used to render email.
        /// </summary>
        public IDictionary<String, Object> Properties { get; private set; }

        /// <summary>
        /// Any attachments you wish to add.  The key of this collection is what
        /// the file should be named.  The value is should represent the binary bytes
        /// of the file.
        /// </summary>
        public AttachmentCollection Attachments { get; private set; }

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
            var notification = message.Deserialize<Notification>();

            this.TemplateName = notification.TemplateName;
            this.From = notification.From;
            this.Subject = notification.Subject;
            this.To = notification.To;
            this.CC = notification.CC;
            this.BCC = notification.BCC;
            this.ReplyTo = notification.ReplyTo;
            this.Properties = notification.Properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            this.Attachments = notification.Attachments.ToDictionary(kvp => kvp.Name, kvp => kvp.ContentStream.ToByteArray()).ToAttachmentCollection();
        }

        public void Execute()
        {
            _mailer.Run(
                    templateName: this.TemplateName,
                    from: this.From,
                    subject: this.Subject,
                    to: this.To,
                    CC: this.CC,
                    BCC: this.BCC,
                    replyTo: this.ReplyTo,
                    properties: this.Properties,
                    attachment: this.Attachments
                    ).DeliverAsync();

        }
    }
}
