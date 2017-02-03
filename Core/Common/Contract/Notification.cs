using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract
{
    [Serializable]
    public class Notification
    {
        public Guid PartitionKey { get; set; }

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
        /// Any attachments you wish to add.  The key of this collection is what
        /// the file should be named.  The value is should represent the binary bytes
        /// of the file.
        /// </summary>
        /// <example>
        /// Attachments["picture.jpg"] = File.ReadAllBytes(@"C:\picture.jpg");
        /// </example>        
        public Collection<Attachment> Attachments { get; private set; }


        /// <summary>
        /// A collection of properties that should be used to render email.
        /// </summary>
        public IList<NotificationProperty> Properties { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public NotificationType Type { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public IList<string> Tags { get; private set; }

        public Notification() : this(NotificationType.Email) { }

        public Notification(NotificationType type)
        {
            Type = type;
            To = new List<String>();
            CC = new List<String>();
            BCC = new List<String>();
            ReplyTo = new List<String>();
            Properties = new List<NotificationProperty>();
            Attachments = new Collection<Attachment>();
            Tags = new List<String>();
        }
    }

}
