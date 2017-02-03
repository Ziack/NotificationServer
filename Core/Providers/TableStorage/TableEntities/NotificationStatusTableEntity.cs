using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class NotificationStatusTableEntity : TableEntity
    {        

        public String Status { get; set; }

        public String Description { get; set; }

        public String ServiceName { get; set; }

        public String Error { get; set; }

        public NotificationStatusTableEntity()
        {
            RowKey = Convert.ToString(Guid.NewGuid());
        }   
    }
}
