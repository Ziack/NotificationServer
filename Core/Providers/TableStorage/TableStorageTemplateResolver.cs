using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Core;
using NotificationServer.Service.TableStorage.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage
{
    public class TableStorageTemplateResolver : TableStorageRepositoryBase, ITemplateResolver
    {
        public override CloudTable Table { get; set; }

        public TableStorageTemplateResolver(String connectionString) : base(connectionString: connectionString)
        {
            this.Table = CloudTableClient.GetTableReference("Templates");
            this.Table.CreateIfNotExists();
        }

        public String Resolve(String name, IDictionary<String, Object> parameters = null)
        {
            var query = Table.CreateQuery<TemplateTableEntity>().Where(t => t.Name == name);

            if (!query.Any())
                throw new TemplateResolvingException($"Template with name { name } does not exist.");

            return query.Single().Body;            
        }
    }
}
