using NotificationServer.Contract.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract.Commands
{
    public static class NotifyCommandExtensionMethods
    {
        public static NotifyCommand ForApplication(this NotifyCommand self, string applicationName)
        {
            self.ApplicationName = applicationName;
            return self;
        }
        public static NotifyCommand WithSubject(this NotifyCommand self, string subject)
        {
            self.Subject = subject;
            return self;
        }

        public static NotifyCommand SentFrom(this NotifyCommand self, string from)
        {
            self.From = from;
            return self;
        }

        public static NotifyCommand WithDestination(this NotifyCommand self, string destinationName, Action<Destination> destinationConfig)
        {
            var destination = new Destination();
            destinationConfig(destination);
            destination.Service = destinationName;
            self.Destinations.Add(destination);
            return self;
        }

        public static NotifyCommand WithProperty(this NotifyCommand self, string key, object value)
        {

            self.Properties.Add(new NotificationProperty(key, value));
            return self;
        }

        public static NotifyCommand WithProperties(this NotifyCommand self, IDictionary<string, object> values)
        {

            self.Properties.AddAll(
                values.Select(vk => new NotificationProperty(vk.Key, vk.Value))
            );

            return self;
        }

        public static NotifyCommand WithTag(this NotifyCommand self, string tag)
        {
            self.Tags.Add(tag);
            return self;
        }
    }

    public static class DestinationExtensionMethdos
    {
        public static Destination AddTo(this Destination self, params string[] tos)
        {
            self.To.AddAll(tos);
            return self;
        }

        public static Destination AddCC(this Destination self, params string[] tos)
        {
            self.CC.AddAll(tos);
            return self;
        }

        public static Destination AddBCC(this Destination self, params string[] tos)
        {
            self.BCC.AddAll(tos);

            return self;
        }

        public static Destination AddReplyTo(this Destination self, params string[] tos)
        {

            self.ReplyTo.AddAll(tos);

            return self;
        }

        public static Destination WithTemplate(this Destination self, string templateName)
        {
            self.TemplateName = templateName;
            return self;
        }

    }
}
