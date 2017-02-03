using Nancy;
using NotificationServer.Service.Events;
using System;
using System.IO;
using System.Linq;

namespace NotificationServer.Nancy.Lib
{
    internal static class NotificationRequestEventFactory
    {
        public static NotificationRequestEvent Crear(
            Request request,
            Response response,
            Exception exception = null,
            string applicationName = ""
        )
        {
            var requestBody = "";
            request.Body.Position = 0;

            using (var reader = new StreamReader(request.Body))
                requestBody = reader.ReadToEnd();

            var message = exception != null ? exception.ToString() : "Invocado exitosamente.";

            var headersRequest = request.Headers;

            var requestString = string.Format(@"
Url: {0}
Headers:
    {1}
Body: {2}",
                request.Url.ToString(),
                string.Join(
                    "\n    ", headersRequest.Select(h => string.Format("{0}: {1}", h.Key, string.Join(";", h.Value)))
                    ),
                requestBody
            );

            var responseString = "";

            return new NotificationRequestEvent
            {
                Application = applicationName,
                Error = message,
                ResultCode = (response != null) ? response.StatusCode.ToString() : "",
                EmitterIP = request.UserHostAddress,
                ReceiverIP = "",
                InputMessage = requestString,
                OutputMessage = responseString,
                Method = request.Method
            };
        }


    }
}
