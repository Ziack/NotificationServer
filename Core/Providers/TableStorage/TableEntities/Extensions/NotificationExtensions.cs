using Newtonsoft.Json;
using NotificationServer.Contract;
using NotificationServer.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities.Extensions
{
    public static class NotificationExtensions
    { 
        public static NotificationTableEntity ToNotifyCommandTableEntity(this NotifyCommand notifyCommand)
        {
            var notificationTableEntity = new NotificationTableEntity
            {
                ApplicationName = notifyCommand.ApplicationName,
                From = notifyCommand.From,
                Subject = notifyCommand.Subject,
                Properties = JsonConvert.SerializeObject(notifyCommand.Properties),
                Destinations = JsonConvert.SerializeObject(notifyCommand.Destinations),
                PartitionKey = Convert.ToString(notifyCommand.Id),               
            };

            return notificationTableEntity;
        }

        public static TagsTableEntity ToTagsTableEntity(this String tagValue, Guid partitionKey)
        {
            var tagsTableEntity = new TagsTableEntity
            {
                 Value = tagValue,
                 PartitionKey = Convert.ToString(partitionKey),                 
            };

            return tagsTableEntity;
        }

        public static AttachmentTableEntity ToAttachmentsTableEntity(this Attachment attachment, Guid partitionKey)
        {
            var tagsTableEntity = new AttachmentTableEntity
            {
                Name = attachment.Name,
                Content = attachment.Content,
                PartitionKey = Convert.ToString(partitionKey),                
            };

            return tagsTableEntity;
        }        
    }
}
