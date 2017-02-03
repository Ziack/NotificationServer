using System;
using System.Collections.Generic;
using NotificationServer.Core.Senders;

namespace NotificationServer.Core.Runtime.Job
{
    public interface INotifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="from">A string representation of who this mail should be from.  Could be your name and email address or just an email address by itself.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="to">A collection of addresses this email should be sent to.</param>
        /// <param name="CC">A collection of addresses that should be CC'ed.</param>
        /// <param name="BCC">A collection of addresses that should be BCC'ed.</param>
        /// <param name="ReplyTo">A collection of addresses that should be listed in Reply-To header.</param>        
        /// <param name="Properties">A collection of properties that should be used to render email.</param>        
        NotificationResult Run(String templateName, String from, String subject, IList<String> to, IList<String> CC, IList<String> BCC, IList<String> replyTo, IDictionary<String, Object> properties, AttachmentCollection attachment);
    }
}
