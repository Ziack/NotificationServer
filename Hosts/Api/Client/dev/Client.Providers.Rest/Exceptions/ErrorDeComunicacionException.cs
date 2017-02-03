using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Client.Providers.Rest.Exceptions
{
    public class ErrorDeComunicacionException : Exception
    {
        public ErrorDeComunicacionException(string message) : base(message)
        {
        }

        public ErrorDeComunicacionException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}
