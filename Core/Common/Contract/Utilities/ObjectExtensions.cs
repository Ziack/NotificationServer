using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract.Utilities
{
    public static class ObjectExtensions
    {
        public static Byte[] ToBinary(this Object source)
        {
            BinaryFormatter bf = new BinaryFormatter();
            byte[] output = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Position = 0;
                bf.Serialize(ms, source);
                output = ms.GetBuffer();
            }
            return output;
        }
    }
}
