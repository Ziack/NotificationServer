using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class ApplicationTableEntity : TableEntity
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public String Token { get; set; }
    }
}
