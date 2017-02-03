using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Core.Utilities;

namespace NotificationServer.Senders.SendGrid
{
    public static class MailMessageExtensions
    {
        public static SendGridMessage ToSendGridMessage(this MailMessage mailMessage)
        {
            var sendGridMessage = new SendGridMessage
                (
                    from: mailMessage.From,
                    to: mailMessage.To.ToArray<MailAddress>(),
                    subject: mailMessage.Subject,
                    html: mailMessage.Body,
                    text: mailMessage.Body
                )
            {
                Cc = mailMessage.CC.ToArray<MailAddress>(),
                Bcc = mailMessage.Bcc.ToArray<MailAddress>(),
                ReplyTo = mailMessage.ReplyToList.ToArray<MailAddress>(),
                StreamedAttachments = mailMessage.Attachments.ToDictionary(kvp => kvp.Name, kvp => kvp.ContentStream.ToMemoryStream())
            };

            return sendGridMessage;

        }
    }
}
