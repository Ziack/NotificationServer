using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NotificationServer.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Utilities;
using NotificationServer.Contract.Commands;

namespace NotificationServer.Client.Providers.AzureStorageQueue
{
    public class AzureStorageQueueNotificationProvider : NotificationProvider
    {
        public override Guid Send(NotifyCommand notification)
        {
            throw new NotImplementedException("This method will be implemented as soon as the Azure Storage and the Windows Service interfaces of notificator pass to the new schema");
        }

        [Obsolete]
        public override void Send(Contract.Notification notification)
        {

            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(this.ConnectionStringName);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference(this.QueueName);

            // Create the queue if it doesn't already exist.
            queue.CreateIfNotExists();

            // Create a message and add it to the queue.
            var message = new CloudQueueMessage(notification.ToBinary());
            queue.AddMessage(message);
        }

        [Obsolete]
        public override Guid Provisioning(ProvisioningCommand provisioning)
        {
            throw new NotImplementedException();
        }
    }
}
