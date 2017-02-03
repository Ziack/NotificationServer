using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage
{
    public abstract class TableStorageRepositoryBase
    {
        public readonly CloudTableClient CloudTableClient;

        public abstract CloudTable Table { get; set; }

        public TableStorageRepositoryBase(string connectionString)
        {            
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            this.CloudTableClient = storageAccount.CreateCloudTableClient();            
        }
    }
}
