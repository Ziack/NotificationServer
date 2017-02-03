using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract.Utilities
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Defines Stream.ReadAllBytes method.
        /// </summary>
        /// <returns>A collection of bytes with the Stream contents</returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            return data;
        }
    }
}
