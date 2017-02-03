using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace NotificationServer.Senders.Mail.Senders
{
    /// <summary>
    /// Implements IMailSender by using System.Net.Mail.SmtpClient.
    /// </summary>
    public class SmtpMailSender : MailSenderBase
    {
        private readonly SmtpClient _client;
        private Action<MailMessage> _callback;
        private string _from;

        public SmtpMailSender(IDictionary<string, object> options)
            : this(new SmtpClient())
        {
            var output = (object)null;
            var output2 = (object)null;

            // TODO: create as private const: Host, Post, Usuario, Password, etc
            if (options.TryGetValue("Host", out output))
                _client.Host = output.ToString();

            if (options.TryGetValue("Port", out output))
                _client.Port = int.Parse(output.ToString());

            if (options.TryGetValue("Usuario", out output) && options.TryGetValue("Password", out output2))
            {
                _client.UseDefaultCredentials = false;
                _client.Credentials = new NetworkCredential(userName: output.ToString(), password: output2.ToString());
            }

            if (options.TryGetValue("SSL", out output) && options.TryGetValue("SSL", out output2) && output2 is bool)
                _client.EnableSsl = (bool)output2;

            if (options.TryGetValue("From", out output))
                _from = output.ToString();
        }

        /// <summary>
        /// Creates a new mail sender based on System.Net.Mail.SmtpClient
        /// </summary>
        /// <param name="client">The underlying SmtpClient instance to use.</param>
        public SmtpMailSender(SmtpClient client)
        {
            _client = client;
        }


        /// <summary>
        /// Sends mail synchronously.
        /// </summary>
        /// <param name="mail">The mail you wish to send.</param>
        public override void Send(MailMessage mail)
        {
            if (mail.From == null || string.IsNullOrEmpty(mail.From.Address))
                mail.From = new MailAddress(_from);

            _client.Send(mail);
        }

        /// <summary>
        /// Sends mail asynchronously.
        /// </summary>
        /// <param name="mail">The mail you wish to send.</param>
        /// <param name="callback">The callback method to invoke when the send operation is complete.</param>
        public override void SendAsync(MailMessage mail, Action<MailMessage> callback)
        {
            if (mail.From == null || string.IsNullOrEmpty(mail.From.Address))
                mail.From = new MailAddress(_from);

            _callback = callback;
            _client.SendCompleted += new SendCompletedEventHandler(AsyncSendCompleted);
            _client.SendAsync(mail, mail);
        }

        private void AsyncSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // unsubscribe from the event so _client can be GC'ed if necessary
            _client.SendCompleted -= AsyncSendCompleted;
            _callback(e.UserState as MailMessage);
        }

        /// <summary>
        /// Destroys the underlying SmtpClient.
        /// </summary>
        public override void Dispose()
        {
            _client.Dispose();
        }
    }
}
