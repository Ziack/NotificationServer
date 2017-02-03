using NotificationServer.Core;
using NotificationServer.Senders.Mail.Senders;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationServer.Senders.SendGrid
{
    /// <summary>
    /// Implements IMailSender by using SendGrid.Web.
    /// </summary>
    public class SendGridMailSender : MailSenderBase
    {
        private readonly Web _client;

        private Action<MailMessage> _callback;

        private MailMessage _mail;

        public SendGridMailSender(IDictionary<string, object> options)
            : this(new NetworkCredential(
                options["Usuario"].ToString(),
                options["Password"].ToString()
            ))
        {

        }

        /// <summary>
        /// Creates a new mail sender based on SendGrid.Web.
        /// </summary>
        /// <param name="credentials"></param>        
        public SendGridMailSender(NetworkCredential credentials)
        {
            _client = new Web(credentials);
        }


        private void AsyncSendCompleted(Task task)
        {
            _callback(_mail);
        }

        /// <summary>
        /// Sends mail synchronously.
        /// </summary>
        /// <param name="mail">The mail you wish to send.</param>
        public override void Send(MailMessage mail)
        {
            _client.Deliver(mail.ToSendGridMessage());
        }

        /// <summary>
        /// Sends mail asynchronously.
        /// </summary>
        /// <param name="mail">The mail you wish to send.</param>
        /// <param name="callback">The callback method to invoke when the send operation is complete.</param>
        public override void SendAsync(MailMessage mail, Action<MailMessage> callback)
        {
            _callback = callback;
            _mail = mail;
            _client.DeliverAsync(mail.ToSendGridMessage()).ContinueWith(AsyncSendCompleted);
        }

        public override void Dispose() { }
    }
}
