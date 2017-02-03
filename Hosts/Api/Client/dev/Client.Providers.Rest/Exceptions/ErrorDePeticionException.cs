using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Client.Providers.Rest.Exceptions
{
    public class ErrorDePeticionException : Exception
    {
        public string ErrorMessage { get; private set; }

        public string ResponseBody { get; private set; }

        public ErrorDePeticionException(HttpStatusCode statusCode, string statusDescription, string errorMessage = null, string responseBody = null)
            : base(string.Format("{0}: {1}", statusCode.ToString(), statusDescription))
        {
            ErrorMessage = errorMessage;
            ResponseBody = responseBody;
        }
    }
}
