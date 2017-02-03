using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class AttachmentTableEntity : TableEntity
    {
        public String Content { get; set; }
        
        public String Name { get; set; }

        public AttachmentTableEntity()
        {
            RowKey = Convert.ToString(Guid.NewGuid());
        }
    }
}
