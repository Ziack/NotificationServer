using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class ServicesByApplicationTableEntity : TableEntity
    {
        public String Service { get; set; }

        public String Name { get; set; }

        public String Configuration { get; set; }
    }
}
