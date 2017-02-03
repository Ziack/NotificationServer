
using System;
using System.Collections.Generic;
namespace NotificationServer.Core.Senders
{
    public class NotificationData
    {
        /// <summary>
        /// A string representation of who this mail should be from.  Could be
        /// your name and email address or just an email address by itself.
        /// </summary>
        public String From { get; set; }

        /// <summary>
        /// The subject line of the email.
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        /// A collection of addresses this email should be sent to.
        /// </summary>
        public IList<String> To { get; private set; }

        /// <summary>
        /// A collection of addresses that should be CC'ed.
        /// </summary>
        public IList<String> CC { get; private set; }

        /// <summary>
        /// A collection of addresses that should be BCC'ed.
        /// </summary>
        public IList<String> BCC { get; private set; }

        /// <summary>
        /// A collection of addresses that should be listed in Reply-To header.
        /// </summary>
        public IList<String> ReplyTo { get; private set; }

        /// <summary>
        /// Any custom headers (name and value) that should be placed on the message.
        /// </summary>
        public IDictionary<String, Object> Properties { get; private set; }

        public NotificationData(String from, String subject, IList<String> to, IList<String> CC, IList<String> BCC, IList<String> replyTo, IDictionary<String, Object> properties = null)
        {
            this.From = from;
            this.To = to;
            this.CC = CC;
            this.BCC = BCC;
            this.ReplyTo = replyTo;
            this.Subject = subject;
            this.Properties = properties;
        }
    }
}
