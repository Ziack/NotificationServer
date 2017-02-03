using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Commands;
using NotificationServer.Service.Entities;
using Insight.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace NotificationServer.Service.SqlServer
{
    public class SqlServerConfigurationsRepository : SqlServerRepositoryBase, IConfigurationsRepository
    {
        public SqlServerConfigurationsRepository()
            : base(null)
        {

        }

        public SqlServerConfigurationsRepository(string connectionString)
            : base(connectionString)
        {

        }

        public IEnumerable<NotificationSpec> GetNotificationSpecsFor(NotifyCommand notification)
        {
            var configurations = Exec(con => con.Query<dynamic>("Notificacion.pr_NotificacionesConfiguracion_List", new
            {
                ds_nombreAplicacion = notification.ApplicationName,
                ds_Servicios = string.Join(",", notification.Destinations.Select(d => d.Service))
            }));

            return configurations.Select<dynamic, NotificationSpec>(
                c => NotificationSpec.FromOptions(Extend(
                    new Dictionary<string, object>
                    {
                        {"Host"     , c.ds_Host     },
                        {"Puerto"   , c.nm_Puerto   },
                        {"Usuario"  , c.ds_Usuario  },
                        {"Password" , c.ds_Password },
                        {"ServiceName", c.nm_Nombre }
                    },
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(c.ds_ConfiguracionBase ?? "{}"),
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(c.ds_ConfiguracionSobreescrita ?? "{}")
                ))
            );
        }

        private IDictionary<string, object> Extend(IDictionary<string, object> extended, IDictionary<string, object> extender, IDictionary<string, object> extender2 = null)
        {
            foreach (var kv in extender)
                extended[kv.Key] = kv.Value;

            if (extender2 != null)
                foreach (var kv in extender2)
                    extended[kv.Key] = kv.Value;

            if (extended.ContainsKey("InterceptorTypes") && extended["InterceptorTypes"] is JArray)
                extended["InterceptorTypes"] = (extended["InterceptorTypes"] as JArray).ToObject<IEnumerable<string>>();

            return extended;
        }
    }
}
