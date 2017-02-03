using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract.Utilities
{
    public static class NotificationExtensions
    {
        public static Notification WithTo(this Notification self, IEnumerable<string> addresses) { self.To.AddAll(addresses); return self; }
        public static Notification WithCC(this Notification self, IEnumerable<string> addresses) { self.CC.AddAll(addresses); return self; }
        public static Notification WithBCC(this Notification self, IEnumerable<string> addresses) { self.BCC.AddAll(addresses); return self; }
        public static Notification WithReplyTo(this Notification self, IEnumerable<string> addresses) { self.ReplyTo.AddAll(addresses); return self; }
        public static Notification WithAttachments(this Notification self, IEnumerable<Attachment> attatchments) { self.Attachments.AddAll(attatchments); return self; }
        public static Notification WithProperties(this Notification self, IEnumerable<NotificationProperty> properties) { self.Properties.AddAll(properties); return self; }
        public static Notification WithTags(this Notification self, IEnumerable<string> tags) { self.Tags.AddAll(tags); return self; }
    }
}
