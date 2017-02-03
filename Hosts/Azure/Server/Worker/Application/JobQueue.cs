using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;
using NotificationServer.Core.Runtime.Queue;
using NotificationServer.Core.Utilities;

namespace NotificationServer.Azure.Application
{
    public class JobQueue : IQueue
    {
        private CloudQueue jobQueue;

        public JobQueue(CloudStorageAccount storageAccount)
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            var queueName = RoleEnvironment.GetConfigurationSettingValue("NotificationServer.QueueName");

            if (string.IsNullOrEmpty(queueName))
                throw new ConfigurationErrorsException("El parametro QueueName no puede estar vacio.");

            jobQueue = queueClient.GetQueueReference(queueName);
            jobQueue.CreateIfNotExists();            
        }

        public CloudQueueMessage GetMessage()
        {
            return jobQueue.GetMessage();
        }

        public void AddMessage(CloudQueueMessage message)
        {
            jobQueue.AddMessage(message);
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            jobQueue.DeleteMessage(message);
        }
    }
}
