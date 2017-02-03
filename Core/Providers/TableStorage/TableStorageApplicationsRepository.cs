using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Service.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Service.TableStorage.TableEntities;
using NotificationServer.Service.TableStorage.TableEntities.Extensions;

namespace NotificationServer.Service.TableStorage
{
    public class TableStorageApplicationsRepository : TableStorageRepositoryBase, IApplicationsRepository
    {
        public override CloudTable Table { get; set; }

        public TableStorageApplicationsRepository(string connectionString) : base(connectionString: connectionString)
        {
            this.Table = CloudTableClient.GetTableReference("Applications");
            this.Table.CreateIfNotExists();
        }


        public IEnumerable<Application> List(int startPage, int pageSize)
        {
            var query = Table.CreateQuery<ApplicationTableEntity>()
                           .Skip(pageSize * startPage).Take(pageSize);


            return query.Select(t => t.ToApplication());
        }

        public void SetToken(string applicationName, string token)
        {
            var query = Table.CreateQuery<ApplicationTableEntity>()
                            .Where(t => t.Name == applicationName);

            if (!query.Any())
                throw new ArgumentException($"Application with name { applicationName } does not exist.");

            var applicationToChange = query.Single();
            applicationToChange.Token = token;

            TableOperation insertOperation = TableOperation.Replace(applicationToChange);
            Table.Execute(insertOperation);
        }
    }
}
