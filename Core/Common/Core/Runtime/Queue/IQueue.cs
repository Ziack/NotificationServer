using Microsoft.WindowsAzure.Storage.Queue;

namespace NotificationServer.Core.Runtime.Queue
{
    public interface IQueue
    {
        CloudQueueMessage GetMessage();

        void AddMessage(CloudQueueMessage message);

        void DeleteMessage(CloudQueueMessage message);
    }
}
