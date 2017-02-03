using Newtonsoft.Json;
using NotificationServer.Contract;
using NotificationServer.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationServer.Senders.SPRC
{
    public class SPRCSender : ISender, IRequireRenderedBody
    {
        private readonly Dictionary<String, Func<String>> _documentTypeResolver = new Dictionary<String, Func<String>>();
        private readonly Dictionary<String, Func<String>> _statusResolver = new Dictionary<String, Func<String>>();
        private readonly Func<String> _defaultStatus = () => { return "R"; };

        public String RenderedBody { get; set; }

        public SPRCSender(IDictionary<String, Object> options)
        {
            var host = (options.ContainsKey("Host")) ? options["Host"].ToString() : "";
            var port = (options.ContainsKey("Port")) ? options["Port"].ToString() : "";
            Url = $"{host}:{port}";

            AuthResource = (options.ContainsKey("Auth.Resource")) ? options["Auth.Resource"].ToString() : "";
            AuthMethod = (options.ContainsKey("Auth.Method")) ? options["Auth.Method"].ToString() : "";

            UpdateResource = (options.ContainsKey("Update.Resource")) ? options["Update.Resource"].ToString() : "";
            UpdateMethod = (options.ContainsKey("Update.Method")) ? options["Update.Method"].ToString() : "";

            Headers = (options.ContainsKey("Headers")) ? (options["Headers"] as Dictionary<String, String>) ?? new Dictionary<String, String>() : new Dictionary<String, String>();

            User = (options.ContainsKey("User")) ? options["User"].ToString() : "";
            Password = (options.ContainsKey("Password")) ? options["Password"].ToString() : "";            
        }

        public String Url { get; set; }
        public String AuthResource { get; set; }
        public String AuthMethod { get; set; }
        public String UpdateResource { get; set; }
        public String UpdateMethod { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
        public Dictionary<String, String> Headers { get; set; }


        public void Send(Notification mail)
        {
            //var authenticationResponse = Authenticate();

            var method = (Method)Enum.Parse(typeof(Method), UpdateMethod, ignoreCase: true);
            var request = new RestRequest(UpdateResource, method);
            request.AddHeader("Authorization", String.Empty);
            foreach (var header in Headers)
                request.AddHeader(header.Key, header.Value);

            request.AddParameter(
                new Parameter
                {
                    ContentType = "application/json",
                    Name = "application/json",
                    Type = ParameterType.RequestBody,
                    Value = RenderedBody
                });

            var client = new RestClient(baseUrl: Url);
            var response = client.Execute(request);
            CheckResponse(response);            
        }

        private AuthenticationResponse Authenticate()
        {
            var method = (Method)Enum.Parse(typeof(Method), AuthMethod, ignoreCase: true);
            var request = new RestRequest(AuthResource, method);
            foreach (var header in Headers)
                request.AddHeader(header.Key, header.Value);

            request.AddJsonBody(new { u = User, p = Password });

            var client = new RestClient(baseUrl: Url);
            var response = client.Execute<AuthenticationResponse>(request);
            CheckResponse(response);

            return response.Data;
        }

        private void CheckResponse(IRestResponse response)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(response.Content);

            //if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
            //    throw new Exception(response.ErrorMessage);

            //if (response.ErrorException != null)
            //    throw new Exception(response.ErrorException.Message, response.ErrorException);
        }

        public void SendAsync(Notification notification, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(notification));
            task.ContinueWith(t => callback(notification));
        }
    }

    internal class AuthenticationResponse
    {
        [JsonProperty("accessToken")]
        public String AccessToken { get; set; }
    }   
}
