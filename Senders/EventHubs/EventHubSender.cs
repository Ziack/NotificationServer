using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NotificationServer.Senders.EventHubs
{
    public class EventHubSender : ISender, IRequireRenderedBody
    {
        public string RenderedBody { get; set; }

        private readonly String _eventHubConnectionString;

        private readonly List<String> _eventsHubs;

        private readonly Dictionary<string, String> _data;

        public EventHubSender(IDictionary<string, object> options)
        {
            this._eventHubConnectionString = (String)options["EventHubConnectionString"];

            this._eventsHubs = new List<String>();

            this._eventsHubs.AddRange((List<String>)options["EventsHub"]);

            var output = (object)null;

            if (options.TryGetValue("Headers", out output))
            {
                var headersJArray = output as JToken;
                if (headersJArray != null)
                    _data = headersJArray.ToObject<Dictionary<string, string>>();
            }

        }
        public EventHubSender() { }

        public void Send(Notification mail)
        {
            var fechaEmisionTikcs = (Convert.ToDateTime(_data["issueDate"])).Date.Ticks.ToString();

            _data.Add("issueDateTikcs", fechaEmisionTikcs);

            this._eventsHubs.ForEach((eventHub) =>
            {

                var eventHubClient = EventHubClient.CreateFromConnectionString(this._eventHubConnectionString, eventHub);
                eventHubClient.Send(
                new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_data)))
                {
                    PartitionKey = "1",

                });

                eventHubClient.Close();
            });
        }

        public void SendAsync(Notification mail, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(mail));
            task.ContinueWith(t => callback(mail));
        }
    }
}
