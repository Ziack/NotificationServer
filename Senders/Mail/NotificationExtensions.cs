using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Senders.Mail
{
    internal static class NotificationExtensions
    {
        public static MailMessage ToMailMessage(this Notification self)
        {
            var mailMessage = new MailMessage
            {
                Subject = self.Subject,
            };

            if (!string.IsNullOrEmpty(self.From))
                mailMessage.From = new MailAddress(self.From);

            mailMessage.To.AddAll(self.To);
            mailMessage.CC.AddAll(self.CC);
            mailMessage.Bcc.AddAll(self.BCC);
            mailMessage.ReplyToList.AddAll(self.ReplyTo);
            mailMessage.Attachments.AddAll(self.Attachments);


            return mailMessage;
        }


        public static Notification ToNotification(this MailMessage self)
        {
            var notification = new Notification(NotificationType.Email)
            {
                From = self.From.Address,
                Subject = self.Subject
            };

            notification.To.AddAll(self.To);
            notification.CC.AddAll(self.CC);
            notification.BCC.AddAll(self.Bcc);
            notification.ReplyTo.AddAll(self.ReplyToList);
            notification.Attachments.AddAll(self.Attachments);

            return notification;
        }

        private static void AddAll(this AttachmentCollection self, IEnumerable<Contract.Attachment> attatchments)
        {
            foreach (var att in attatchments)
                self.Add(new System.Net.Mail.Attachment(att.ContentStream, att.Name));
        }

        private static void AddAll(this MailAddressCollection self, IEnumerable<string> addressList)
        {

            if (addressList == null)
                return;

            foreach (var address in addressList)
            {
                if (!String.IsNullOrEmpty(address))
                    self.Add(new MailAddress(address));
            }            
        }

        private static void AddAll(this Collection<Contract.Attachment> self, IEnumerable<System.Net.Mail.Attachment> attatchments)
        {
            foreach (var att in attatchments)
                self.Add(new Contract.Attachment(att.ContentStream, att.Name));
        }

        private static void AddAll(this IList<string> self, IEnumerable<MailAddress> addresses)
        {
            foreach (var address in addresses)
                self.Add(address.Address);
        }
    }
}
