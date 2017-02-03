using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Commands;
using NotificationServer.Service.Commands;
using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Service.TableStorage.TableEntities;
using NotificationServer.Contract;
using Newtonsoft.Json;
using NotificationServer.Service.TableStorage.TableEntities.Extensions;

namespace NotificationServer.Service.TableStorage
{
    public class TableStorageNotificationsRepository : TableStorageRepositoryBase, INotificationsRepository
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };

        public override CloudTable Table { get; set; }

        public CloudTable TagsTable { get; set; }

        public CloudTable AttachmentsTable { get; set; }

        public CloudTable NotificationStatus { get; set; }

        public CloudTable NotificationBatch { get; set; }

        public TableStorageNotificationsRepository(string connectionString) : base(connectionString: connectionString)
        {
            this.Table = CloudTableClient.GetTableReference("Notifications");
            this.TagsTable = CloudTableClient.GetTableReference("Tags");
            this.AttachmentsTable = CloudTableClient.GetTableReference("Attachments");
            this.NotificationStatus = CloudTableClient.GetTableReference("NotificationStatus");
            this.NotificationBatch = CloudTableClient.GetTableReference("NotificationBatch");

            this.Table.CreateIfNotExists();
            this.TagsTable.CreateIfNotExists();
            this.AttachmentsTable.CreateIfNotExists();
            this.NotificationStatus.CreateIfNotExists();
            this.NotificationBatch.CreateIfNotExists();
        }

        public Guid Save(NotifyCommand notification)
        {
            TableOperation insertNotificationOperation = TableOperation.Insert(notification.ToNotifyCommandTableEntity());
            Table.Execute(insertNotificationOperation);

            foreach (var tag in notification.Tags)
            {
                TableOperation insertTagsOperation = TableOperation.Insert(tag.ToTagsTableEntity(partitionKey: notification.Id));
                TagsTable.Execute(insertTagsOperation);
            }

            foreach (var attachment in notification.Attachments)
            {
                TableOperation insertTagsOperation = TableOperation.Insert(attachment.ToAttachmentsTableEntity(partitionKey: notification.Id));
                AttachmentsTable.Execute(insertTagsOperation);
            }

            return notification.Id;
        }

        public Guid AddToBatch(Guid notificationId, Guid batchId)
        {
            TableOperation insertNotificationOperation = TableOperation.Insert
                (
                    new NotificationBatchTableEntity
                    {
                        RowKey = Convert.ToString(notificationId),
                        PartitionKey = Convert.ToString(batchId)
                    }
                );
            NotificationBatch.Execute(insertNotificationOperation);


            return batchId;
        }

        public IEnumerable<NotifyCommand> GetBatch(Guid batchId)
        {
            var notificationQuery = Table.CreateQuery<NotificationBatchTableEntity>()
                            .Where(t => t.PartitionKey == Convert.ToString(batchId))
                            .ToList();


            foreach (var item in notificationQuery.Select( t => Guid.Parse(t.RowKey)))
            {
                yield return Get(item);
            }
        }

        public NotifyCommand Get(Guid messageId)
        {
            var notificationQuery = Table.CreateQuery<NotificationTableEntity>()
                            .Where(t => t.RowKey == Convert.ToString(messageId))
                            .ToList();

            if (!notificationQuery.Any())
                throw new ArgumentException($"Notification with id { messageId } does not exist.");

            var notificationResult = notificationQuery.SingleOrDefault();
            var destinations = JsonConvert.DeserializeObject<List<Destination>>(notificationResult.Destinations, _settings);
            var properties = JsonConvert.DeserializeObject<List<NotificationProperty>>(notificationResult.Properties, _settings);

            var tagsQuery = TagsTable.CreateQuery<TagsTableEntity>()
                            .Where(t => t.PartitionKey == notificationResult.PartitionKey)
                            .ToList();

            var attachmentQuery = AttachmentsTable.CreateQuery<AttachmentTableEntity>()
                            .Where(t => t.PartitionKey == notificationResult.PartitionKey)
                            .ToList();

            var notification = new NotifyCommand
            {
                Id = Guid.Parse(notificationResult.PartitionKey),
                Destinations = destinations,
                From = notificationResult.From,
                Subject = notificationResult.Subject,
                ApplicationName = notificationResult.ApplicationName,
                Properties = properties,
                Attachments = Enumerable.Empty<Attachment>().ToList(),
                Tags = tagsQuery.Select( t => t.Value).ToList()
            };

            return notification;
        }

        public void ReportNotificationStatus(ReportNotificationStatusCommand status)
        {
            var notificationStatus = new NotificationStatusTableEntity
            {
                Description = status.Description,
                Error = status.Error?.Message,
                PartitionKey = Convert.ToString(status.NotificationId),
                ServiceName = status.ServiceName,
                Status = status.Status
            };

            TableOperation insertNotificationOperation = TableOperation.Insert(notificationStatus);
            NotificationStatus.Execute(insertNotificationOperation);
        }        
    }
}
