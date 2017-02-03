using NotificationServer.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Commands;
using RestSharp;
using System.Net;
using NotificationServer.Client.Providers.Rest.Exceptions;
using NotificationServer.Client.Providers.Rest.Utils;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using NotificationServer.Contract.Utilities;

namespace NotificationServer.Client.Providers.Rest
{
    public class RestNotificationProvider : NotificationProvider
    {
        private static readonly Regex _connectionStringRegex = new Regex(
            @"^((http|https):\/\/(.+))#(.+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline
        );

        private Uri _notifierUrl;
        private string _notifierToken;

        public override Guid Send(NotifyCommand notification)
        {
            var client = new RestClient(_notifierUrl);
            var response = client.Execute<Guid>(CreateRequest(notification));

            CheckResponse(response);

            return response.Data;
        }

        public override Guid Provisioning(ProvisioningCommand provisioning)
        {
            var client = new RestClient(_notifierUrl);
            var response = client.Execute<Guid>(CreateRequest(provisioning));

            CheckResponse(response);

            return response.Data;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            var match = _connectionStringRegex.Match(ConnectionStringName);

            if (match.Groups.Count != 5)
                throw new Exception("La cadena de conexión no cumple el formato urlNotificador#token");

            _notifierUrl = new Uri(match.Groups[1].ToString());
            _notifierToken = match.Groups[4].ToString();
        }

        private IRestRequest CreateRequest(NotifyCommand notification)
        {
            var request = new  RestRequest("/Notifications", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("X-Token", _notifierToken);

            request.JsonSerializer = new JsonSerializer();
            
            var body = new NotificationViewModel
            {
                Id = notification.Id,
                Destinations = notification.Destinations.ToDictionary(d => d.Service, d => new DestinationViewModel
                {
                    TemplateName = d.TemplateName,
                    To = d.To,
                    CC = d.CC,
                    BCC = d.BCC,
                    ReplyTo = d.ReplyTo
                }),
                From = notification.From,
                Properties = notification.Properties.Select(p => new Property
                {
                    Key = p.Key,
                    Value = p.Value
                }).ToList(),
                Subject = notification.Subject,
                Tags = notification.Tags
            };

            foreach (var attachment in notification.Attachments)
            {
                request.AddFile(name: attachment.Name, bytes: attachment.ContentStream.ReadAllBytes(), fileName: attachment.Name, contentType: String.Empty);
            }

            request.AddJsonBody(body);

            return request;
        }

        private IRestRequest CreateRequest(ProvisioningCommand provisioning)
        {
            var request = new RestRequest("/Provisioning", Method.POST)
                .AddHeader("Accept", "application/json")
                .AddHeader("X-Token", _notifierToken);

            request.JsonSerializer = new JsonSerializer();

            var body = new ProvisioningViewModel
            {
               
            };

            request.AddJsonBody(body);

            return request;
        }

        private void CheckResponse(IRestResponse response)
        {
            if (_badStatusCodes.Contains(response.StatusCode))
                throw new ErrorDePeticionException(response.StatusCode, response.StatusDescription, response.ErrorMessage, response.Content);

            if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
                throw new ErrorDeComunicacionException(response.ErrorMessage);

            if (response.ErrorException != null)
                throw new ErrorDeComunicacionException(response.ErrorException);
        }

        #region private static readonly IEnumerable<HttpStatusCode> _badStatusCodes = new HttpStatusCode[] {....
        private static readonly IEnumerable<HttpStatusCode> _badStatusCodes = new HttpStatusCode[]{
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
}
