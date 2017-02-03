using NotificationServer.Senders.Mail.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Amazon.SimpleEmail;
using Amazon;
using Amazon.SimpleEmail.Model;

namespace NotificationServer.Senders.Amazon
{
    public class SESMailSenser : MailSenderBase
    {
        private AmazonSimpleEmailServiceClient _client;
        private string _from;

        public SESMailSenser(IDictionary<string, object> options)
        {
            var region = default(object);
            var user = default(object);
            var password = default(object);
            var from = default(object);
            /*
                <add key="AWSAccessKey" value="AKIAIY35X5IHHZOF2F2A" />
                <add key="AWSSecretKey" value="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" />
                <add key="AWSRegion" value="us-east-1"/>
            */

            if (!(options.TryGetValue("Host", out region) || options.TryGetValue("AWSRegion", out region)))
                region = "us-east-1";


            if (!(options.TryGetValue("Usuario", out user) || options.TryGetValue("AWSAccessKey", out user)))
                throw new ArgumentException("Falta el parámetro 'AWSAccessKey'.");

            if (!(options.TryGetValue("Password", out password) || options.TryGetValue("AWSSecretKey", out password)))
                throw new ArgumentException("Falta el parámetro 'AWSSecretKey'.");

            if (!options.TryGetValue("From", out from))
                throw new ArgumentException("Falta el parámetro 'From'.");

            _from = from.ToString();


            _client = new AmazonSimpleEmailServiceClient(user.ToString(), password.ToString(), RegionEndpoint.GetBySystemName(region.ToString()));

        }

        public override void Dispose()
        {
            _client.Dispose();
        }

        public override void Send(MailMessage mail)
        {
            mail.From = new MailAddress(_from);
            // Send the email.
            _client.SendEmail(ToSendEmailRequest(mail, RenderedBody));
        }

        public override void SendAsync(MailMessage mail, Action<MailMessage> callback)
        {
            mail.From = new MailAddress(_from);

            var task = _client.SendEmailAsync(ToSendEmailRequest(mail, RenderedBody));
            task.ContinueWith(r => callback(mail));
        }

        private static SendEmailRequest ToSendEmailRequest(MailMessage mail, string renderedBody)
        {
            var destination = new Destination
            {
                ToAddresses = mail.To.Select(t => t.Address).ToList(),
                CcAddresses = mail.CC.Select(t => t.Address).ToList(),
                BccAddresses = mail.Bcc.Select(t => t.Address).ToList(),
            };

            var from = mail.From.Address;

            var replyTo = mail.ReplyToList.Select(r => r.Address).ToList();

            var body = new Body
            {
                Html = new Content(renderedBody)
            };

            // Create a message with the specified subject and body.
            var message = new Message(new Content(mail.Subject), body);

            // Assemble the email.
            return new SendEmailRequest
            {
                Source = from,
                ReplyToAddresses = replyTo,
                Destination = destination,
                Message = message,
            };
        }
    }
}
