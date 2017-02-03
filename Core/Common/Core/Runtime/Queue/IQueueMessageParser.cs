using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Runtime.Queue
{
    public interface IQueueMessageParser
    {
        void Parse(CloudQueueMessage message);
    }
}
