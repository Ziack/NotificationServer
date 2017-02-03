using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Azure.Application
{
    public static class CloudQueueMessageExtensions
    {
        public static T FromMessage<T>(this CloudQueueMessage m)
        {
            byte[] buffer = m.AsBytes;
            T returnValue = default(T);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                ms.Position = 0;
                BinaryFormatter bf = new BinaryFormatter();
                returnValue = (T)bf.Deserialize(ms);
            }
            return returnValue;
        }
    }
}
