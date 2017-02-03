using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class ServicesTableEntity : TableEntity
    {
        public String Name { get; set; }

        public String Description { get; set; }

        public String Configuration { get; set; }

        public ServicesTableEntity()
        {
            RowKey = Convert.ToString(Guid.NewGuid());
        }
    }
}
