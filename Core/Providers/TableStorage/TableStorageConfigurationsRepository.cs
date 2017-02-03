using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Commands;
using NotificationServer.Service.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Service.TableStorage.TableEntities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NotificationServer.Service.TableStorage
{
    public class TableStorageConfigurationsRepository : TableStorageRepositoryBase, IConfigurationsRepository
    {
        public override CloudTable Table { get; set; }
        public CloudTable ServicesByApplicationTable { get; set; }
        public CloudTable ApplicationsTable { get; set; }

        public TableStorageConfigurationsRepository(string connectionString) : base(connectionString: connectionString)
        {
            this.Table = CloudTableClient.GetTableReference("Services");
            this.ServicesByApplicationTable = CloudTableClient.GetTableReference("ServicesByApplication");
            this.ApplicationsTable = CloudTableClient.GetTableReference("Applications");

            this.Table.CreateIfNotExists();
            this.ServicesByApplicationTable.CreateIfNotExists();
            this.ApplicationsTable.CreateIfNotExists();
        }

        public IEnumerable<NotificationSpec> GetNotificationSpecsFor(NotifyCommand notification)
        {
            var queryApplication = ApplicationsTable.CreateQuery<ApplicationTableEntity>()
                                .Where(t => t.Name == notification.ApplicationName)
                                .ToList();

            if (!queryApplication.Any())
                throw new ArgumentException($"Application with name { notification.ApplicationName } does not exist.");

            var application = queryApplication.Single();

            var intendedServices = notification.Destinations.Select(d => d.Service).ToArray();

            var baseConfigurationsQuery = Table.CreateQuery<ServicesTableEntity>();
            Array.ForEach(intendedServices, (item) => baseConfigurationsQuery.Where(t => t.Name == item));
            var baseConfiguration = baseConfigurationsQuery.ToList();

            var configurationsQuery = ServicesByApplicationTable.CreateQuery<ServicesByApplicationTableEntity>();
            Array.ForEach(intendedServices, (item) => configurationsQuery.Where(t => t.Name == item && t.PartitionKey == application.RowKey));

            var configurations = from cfg in configurationsQuery.ToList()
                                 join baseCfg in baseConfiguration on cfg.Service equals baseCfg.Name
                                 select new
                                 {
                                     ServiceName = cfg.Name,
                                     BaseConfiguration = baseCfg.Configuration,
                                     Configuration = cfg.Configuration
                                 };


            return configurations.Select<dynamic, NotificationSpec>(
                c => NotificationSpec.FromOptions(Extend(
                    new Dictionary<string, object>
                    {
                        {"ServiceName", c.ServiceName },
                        { "BatchId", notification.Properties.Any(t => t.Key == "BatchId") ? notification.Properties.Single(t => t.Key == "BatchId").Value : null }
                    },
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(c.BaseConfiguration ?? "{}"),
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(c.Configuration ?? "{}")
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
