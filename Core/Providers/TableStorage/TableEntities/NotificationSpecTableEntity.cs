using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class NotificationSpecTableEntity : TableEntity
    {
        public Guid MessageId { get; set; }

        public String ServiceName { get; set; }

        public SchedulingType Scheduling { get; set; }

        public SchedulingPriority Priority { get; set; }

        public Encoding Encoding { get; set; }

        public IDictionary<String, Object> Options { get; set; }

        public String SenderType { get; set; }

        public IEnumerable<String> InterceptorTypes { get; set; }
    }
}
