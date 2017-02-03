using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class TagsTableEntity : TableEntity
    {
        public String Value { get; set; }

        public TagsTableEntity()
        {
            RowKey = Convert.ToString(Guid.NewGuid());
        }
    }
}
