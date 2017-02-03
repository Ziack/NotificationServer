using Newtonsoft.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Client.Providers.Rest.Utils
{
    public class JsonSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializerSettings _serializerSettings;

        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public JsonSerializer()
        {
            ContentType = "application/json";
            _serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
            };
        }

        public JsonSerializer(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            ContentType = "application/json";
            _serializerSettings = settings;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _serializerSettings);
        }

    }
}
