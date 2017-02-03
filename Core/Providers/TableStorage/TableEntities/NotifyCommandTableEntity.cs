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
    public class NotifyCommandTableEntity : TableEntity
    {
        public Guid Id { get; set; }

        public String ApplicationName { get; set; }

        public List<Destination> Destinations { get; set; }

        public String From { get; set; }

        public String Subject { get; set; }        

        public List<Attachment> Attachments { get; set; }

        public List<String> Tags { get; set; }

        public String Response { get; set; }

        public String urlSender { get; set; }
    }
}
