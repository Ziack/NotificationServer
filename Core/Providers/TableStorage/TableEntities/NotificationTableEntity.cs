using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Contract;
using NotificationServer.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class NotificationTableEntity : TableEntity
    {
        public String From { get; set; }

        public String Subject { get; set; }

        public String ApplicationName { get; set; }

        public String Destinations { get; set; }

        public String Properties { get; set; }

        public NotificationTableEntity()
        {
            RowKey = Convert.ToString(Guid.NewGuid());
        }
    }
}
