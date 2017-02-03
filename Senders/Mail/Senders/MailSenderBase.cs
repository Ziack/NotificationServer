using log4net;
using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using NotificationServer.Contract;

namespace NotificationServer.Senders.Mail.Senders
{
    /// <summary>
    /// This is a standalone MailerBase that relies on RazorEngine to generate emails.
    /// </summary>
    public abstract class MailSenderBase : IMailSender
    {
        private static readonly ILog _log = LogManager.GetLogger(Globals.LOG4NET_WORKFLOW_SERVER_API_SOURCE);

        public string RenderedBody { get; set; }

        public void Send(Notification notification)
        {
            Send(CreateMailMessage(notification));
        }

        public void SendAsync(Notification notification, Action<Notification> callback)
        {
            SendAsync(CreateMailMessage(notification), m => callback(m.ToNotification()));
        }

        /// <summary>
        /// Sends mail synchronously.
        /// </summary>
        /// <param name="mail">The mail message you wish to send.</param>
        public abstract void Send(MailMessage mail);


        /// <summary>
        /// Sends mail asynchronously.
        /// </summary>
        /// <param name="mail">The mail message you wish to send.</param>
        /// <param name="callback">The callback method that will be fired when sending is complete.</param>
        public abstract void SendAsync(MailMessage mail, Action<MailMessage> callback);

        public abstract void Dispose();

        private MailMessage CreateMailMessage(Notification notification)
        {
            var mail = notification.ToMailMessage();

            mail.Body = RenderedBody;

            mail.IsBodyHtml = true;

            return mail;
        }

    }
}
