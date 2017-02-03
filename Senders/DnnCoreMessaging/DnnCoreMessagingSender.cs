using Newtonsoft.Json;
using NotificationServer.Contract;
using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NotificationServer.Senders.DnnCoreMessaging
{
    public class DnnCoreMessagingSender : ISender, IRequireRenderedBody
    {
        public string RenderedBody { get; set; }

        public DnnCoreMessagingSender(IDictionary<string, object> options)
        {
            Host = (options.ContainsKey("Host")) ? options["Host"].ToString() : "";
            Resource = (options.ContainsKey("Resource")) ? options["Resource"].ToString() : "/DesktopModules/InternalServices/API/MessagingService/Create";
            User = (options.ContainsKey("User")) ? options["User"].ToString() : "";
            Password = (options.ContainsKey("Password")) ? options["Password"].ToString() : "";
            ModuleId = (options.ContainsKey("ModuleId")) ? options["ModuleId"].ToString() : "511";
            TabId = (options.ContainsKey("TabId")) ? options["TabId"].ToString() : "98";
            RequestVerificationToken = (options.ContainsKey("RequestVerificationToken")) ? options["RequestVerificationToken"].ToString(): "";
            CookieRequestVerificationToken = (options.ContainsKey("CookieRequestVerificationToken")) ? options["CookieRequestVerificationToken"].ToString(): "";
        }

        public string Host { get; private set; }
        public string Resource { get; private set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ModuleId { get; set; }
        public string TabId { get; set; }
        public string RequestVerificationToken { get; set; }
        public string CookieRequestVerificationToken { get; set; }

        public void Send(Notification notification)
        {
            CookieContainer gaCookies = new CookieContainer();
            gaCookies.Add(new Uri(Host), new Cookie("__RequestVerificationToken", CookieRequestVerificationToken));

            using (var handler = new HttpClientHandler() { CookieContainer = gaCookies, Credentials = new NetworkCredential(User, Password) })
            {
                using (var client = new HttpClient(handler))
                {
                    var jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.None
                    };

                    var param = JsonConvert.SerializeObject(new
                    {
                        body = RenderedBody,
                        fileIds = new { },
                        roleIds = new { },
                        subject = string.Format("{0}#{1}#", notification.Subject, string.Join("#", notification.Tags)),
                        userIds = JsonConvert.SerializeObject(notification.To.ToArray(), jsonSerializerSettings)
                    }, jsonSerializerSettings);

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(string.Format("{0}{1}", Host, Resource)),
                        Method = HttpMethod.Post,
                    };

                    request.Content = new StringContent(param);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    request.Headers.Add("RequestVerificationToken", RequestVerificationToken);
                    request.Headers.Add("ModuleId", ModuleId);
                    request.Headers.Add("TabId", TabId);

                    var task = client.SendAsync(request);

                    task.Wait();

                    var result = task.Result;
                }
            }
        }

        public void SendAsync(Notification notification, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(notification));
            task.ContinueWith(t => callback(notification));
        }
    }
}
