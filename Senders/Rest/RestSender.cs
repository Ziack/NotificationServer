using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;
using NotificationServer.Client.Providers.Rest.Exceptions;
using NotificationServer.Contract.Utilities;
using System.Web;

namespace NotificationServer.Senders.Rest
{
    public class RestSender : ISender, IRequireRenderedBody, IYieldResponse
    {
        private object _response;

        public string RenderedBody { get; set; }

        public Endpoint Endpoint { get; set; }

        public object Response
        {
            get { return _response; }
        }

        public String UrlSender
        {
            get { return Endpoint.Url; }
        }

        public RestSender() { }

        public RestSender(Endpoint endpoint, string renderedBody = null)
        {
            Endpoint = endpoint;
            RenderedBody = renderedBody;
        }

        public RestSender(IDictionary<string, object> options)
        {
            Endpoint = GetEndpoint(options);

        }

        public void Send(Notification notification)
        {
            //if (Endpoint.Enabled)
                InvokeService(endpoint: Endpoint, body: RenderedBody, attachments: notification.Attachments);
        }


        public void SendAsync(Notification notification, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(notification));
            task.ContinueWith(t => callback(notification));
        }

        private void InvokeService(Endpoint endpoint, string body, IEnumerable<Attachment> attachments)
        {
            var client = new RestClient(new Uri(endpoint.Url));
            var response = client.Execute(endpoint.CreateRequest(body: body, attachments: attachments));

            _response = response.Content;

            CheckResponse(response);
        }

        private Endpoint GetEndpoint(IDictionary<string, object> options)
        {
            var output = (object)null;
            var host = "";
            var resource = "/";
            var port = "80";
            var enabled = true;
            var headers = new Dictionary<string, string>();
            var queryString = new Dictionary<string, string>();
            var contentType = "application/json";
            var method = "POST";

            if (options.TryGetValue("Enabled", out output))
                enabled = bool.Parse(output.ToString());

            if (options.TryGetValue("Host", out output))
                host = output.ToString();

            if (options.TryGetValue("Port", out output))
                port = output.ToString();

            if (options.TryGetValue("Resource", out output))
                resource = output.ToString();

            if (options.TryGetValue("Method", out output))
                method = output.ToString();

            if (options.TryGetValue("ContentType", out output))
                contentType = output.ToString();

            if (options.TryGetValue("Headers", out output))
            {
                var headersJArray = output as JToken;
                if (headersJArray != null)
                    headers = headersJArray.ToObject<Dictionary<string, string>>();
            }

            if (options.TryGetValue("Query", out output))
            {
                var queryStringJArray = output as JToken;
                if (queryStringJArray != null)
                    queryString = queryStringJArray.ToObject<Dictionary<string, string>>();
            }

            return new Endpoint
            {
                Enabled = enabled,
                Url = string.Format("{0}:{1}", host, port),
                Resource = resource,
                QueryString = queryString,
                ContentType = contentType,
                Headers = headers,
                Method = method
            };
        }

        public static void CheckResponse(IRestResponse response)
        {
            if (_codigosHttpDeError.Contains(response.StatusCode))
                throw new ErrorDeComunicacionException(response.Content);            

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
                throw new ErrorDeComunicacionException(response.ErrorMessage);

            if (response.ErrorException != null)
                throw new ErrorDeComunicacionException(response.ErrorException);
        }

        #region private static readonly IEnumerable<HttpStatusCode> _codigosHttpDeError = new HttpStatusCode[] {....
        private static readonly IEnumerable<HttpStatusCode> _codigosHttpDeError = new HttpStatusCode[]{
            HttpStatusCode.BadRequest,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.PaymentRequired,
            HttpStatusCode.Forbidden,
            HttpStatusCode.NotFound,
            HttpStatusCode.MethodNotAllowed,
            HttpStatusCode.NotAcceptable,
            HttpStatusCode.ProxyAuthenticationRequired,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.Conflict,
            HttpStatusCode.Gone,
            HttpStatusCode.LengthRequired,
            HttpStatusCode.PreconditionFailed,
            HttpStatusCode.RequestEntityTooLarge,
            HttpStatusCode.RequestUriTooLong,
            HttpStatusCode.UnsupportedMediaType,
            HttpStatusCode.RequestedRangeNotSatisfiable,
            HttpStatusCode.ExpectationFailed,
            HttpStatusCode.UpgradeRequired,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.NotImplemented,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.HttpVersionNotSupported,
        };
        #endregion
    }

    public struct Endpoint
    {
        public bool Enabled { get; set; }

        public string Url { get; set; }

        public string Resource { get; set; }

        public string Method { get; set; }

        public string ContentType { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public IDictionary<string, string> QueryString { get; set; }

        public IRestRequest CreateRequest(String body, IEnumerable<Attachment> attachments)
        {
            var method = (Method)Enum.Parse(typeof(Method), Method, ignoreCase: true);
            var request = new RestRequest(Resource, method);

            foreach (var header in Headers)
                request.AddHeader(header.Key, header.Value);

            foreach (var query in QueryString)
                request.AddQueryParameter(query.Key, query.Value);

            request.AddParameter(
                new Parameter
                {
                    ContentType = ContentType ?? "application/json",
                    Name = ContentType ?? "application/json",
                    Type = ParameterType.RequestBody,
                    Value = body
                });

            foreach (var attachment in attachments)
            {
                request.AddFile(
                    name: attachment.Name,
                    bytes: attachment.ContentStream.ReadAllBytes(),
                    fileName: attachment.Name,
                    contentType: MimeMapping.GetMimeMapping(attachment.Name));
            }

            return request;
        }
    }
}
