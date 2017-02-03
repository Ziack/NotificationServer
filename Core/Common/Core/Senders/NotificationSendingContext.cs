using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// A special context object used by the OnMailSending() method
    /// to allow you to inspect the underlying MailMessage before it
    /// is sent, or prevent it from being sent altogether.
    /// </summary>
    public class NotificationSendingContext
    {
        public readonly ISender Sender;

        /// <summary>
        /// The generated mail message that is being sent.
        /// </summary>
        public readonly Notification Notification;

        /// <summary>
        /// The used mail template that is being sent.
        /// </summary>
        public readonly IDictionary<String, Object> Parameters;

        /// <summary>
        /// A special flag that you can toggle to prevent this mail
        /// from being sent.
        /// </summary>
        public bool Cancel;

        /// <summary>
        /// Returns a populated context to be used for the OnMailSending()
        /// method in MailerBase.
        /// </summary>
        /// <param name="notification">The message you wish to wrap within this context.</param>
        public NotificationSendingContext(Notification notification, IDictionary<String, Object> parameters, ISender sender)
        {
            Notification = notification;
            Parameters = parameters;
            Cancel = false;
            Sender = sender;
        }
    }
}
