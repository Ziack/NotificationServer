using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;
using Insight.Database;
using System.Data;
using System.Configuration;
using NotificationServer.Service.SqlServer.Config;
using Newtonsoft.Json;
using System.IO;
using NotificationServer.Contract.Utilities;
using NotificationServer.Service.Commands;
using NotificationServer.Contract.Commands;

namespace NotificationServer.Service.SqlServer
{
    public class SqlServerNotificationsRepository : SqlServerRepositoryBase, INotificationsRepository
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };

        public SqlServerNotificationsRepository()
            : base(null)
        { }

        public SqlServerNotificationsRepository(string connectionString)
             : base(connectionString)
        { }

        public NotifyCommand Get(Guid messageId)
        {
            var result = Exec(con => con.QueryResults<dynamic, dynamic, dynamic>("Notificacion.pr_Notificaciones_Get", new
            {
                uid_notificacion = messageId
            }));

            var notificationResult = result.Set1.First();

            var destinations = JsonConvert.DeserializeObject<List<Destination>>(notificationResult.ds_json_to, _settings);
            var properties = JsonConvert.DeserializeObject<List<NotificationProperty>>(notificationResult.ds_json_propiedades, _settings);
            var attatchments = result.Set2.Select(attachment => new Attachment(new MemoryStream(attachment.bin_datos), attachment.nm_nombre)).ToList();
            var tags = result.Set3.Select(tag => tag.nm_tag.ToString()).Cast<string>().ToList();

            var notification = new NotifyCommand
            {
                Id = notificationResult.uid_PartitionKey,
                Destinations = destinations,
                From = notificationResult.nm_from,
                Subject = notificationResult.ds_subject,
                ApplicationName = notificationResult.nm_nombre_aplicacion,
                Properties = properties,
                Attachments = attatchments,
                Tags = tags
            };

            return notification;
        }

        public void ReportNotificationStatus(ReportNotificationStatusCommand status)
        {
            Exec(con => con.Execute("Notificacion.pr_EventosNotificaciones_Save", new
            {
                uid_Notificacion = status.NotificationId,
                cd_NombreEstado = status.Status,
                ds_DescripcionEstado = status.Description,
                cd_NombreServicio = status.ServiceName,
                ds_Error = status.Error != null ? status.Error.ToString() : null,
                id_Usuario = 1,
                uid_PartitionKey = status.PartitionKey
            }, closeConnection: true));
        }

        public Guid Save(NotifyCommand notification)
        {
            var uid = SaveInternal(notification);

            if (notification.Attachments != null)
                SaveAttachments(uid, notification.Attachments);


            if (notification.Tags != null)
                SaveTags(uid, notification.Tags);


            return uid;
        }

        public Guid AddToBatch(Guid notificationId, Guid batchId)
        {
            throw new NotImplementedException(); ; 
        }


        private void SaveTags(Guid uid, IEnumerable<string> tags)
        {
            if (tags.Count() > 0)
                Exec(con => con.Execute("Notificacion.pr_Tags_Save", new
                {
                    uid_notificacion = uid,
                    ds_tags = string.Join(",", tags),
                    id_usuario = 1
                }));
        }

        private void SaveAttachments(Guid uid, IEnumerable<Attachment> attachments)
        {
            foreach (var attatchment in attachments)
            {
                using (var ms = new MemoryStream())
                {
                    attatchment.ContentStream.CopyTo(ms);
                    var data = ms.ToArray();
                    Exec(con => con.Execute("Notificacion.pr_Adjuntos_Save", new
                    {
                        uid_notificacion = uid,
                        nm_nombre = attatchment.Name,
                        bin_datos = data,
                        id_usuario = 1,
                    }));
                }
            }
        }

        private Guid SaveInternal(NotifyCommand notification)
        {
            return Exec(con => con.ExecuteScalar<Guid>("Notificacion.pr_Notificaciones_Save", new
            {
                nm_from = notification.From,
                nm_nombre_aplicacion = notification.ApplicationName,
                ds_subject = notification.Subject,
                ds_json_to = JsonConvert.SerializeObject(notification.Destinations, _settings),
                ds_json_propiedades = JsonConvert.SerializeObject(notification.Properties, _settings),
                id_Usuario = 1,
                uid_PartitionKey = notification.Id
            }, closeConnection: true));
        }
    }
}
