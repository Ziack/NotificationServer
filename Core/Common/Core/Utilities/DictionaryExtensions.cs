using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Utilities
{
    public static class DictionaryExtensions
    {
        //  User-defined conversion from double to Digit 
        public static AttachmentCollection ToAttachmentCollection(this Dictionary<string, byte[]> attachments)
        {
            var collection = new AttachmentCollection();
            Parallel.ForEach(attachments, item =>
            {
                collection.Add(item.Key, item.Value);
            });

            return collection;
        }
    }
}
