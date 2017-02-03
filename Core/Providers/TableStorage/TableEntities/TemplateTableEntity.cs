using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class TemplateTableEntity : TableEntity
    {
        public String Name { get; set; }

        public String Body { get; set; }
    }
}
