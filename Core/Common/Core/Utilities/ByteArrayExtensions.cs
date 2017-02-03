using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Utilities
{
    public static class ByteArrayExtensions
    {
        public static TResult Deserialize<TResult>(this byte[] message)
        {
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            
            memStream.Write(message, 0, message.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            return (TResult)binForm.Deserialize(memStream);
        }
    }
}
