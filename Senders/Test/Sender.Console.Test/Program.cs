using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NotificationServer.Contract;
using System.Net.Mail;
using System.Net;
using SendGrid;
using System.Collections;
using NotificationServer.Client;
using System.Xml;



namespace NotificationServer.Sender.Console.Test
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            Notification notification = new Notification(NotificationType.Email)
            {
                From = "noreply@fakedomain.com",
                Subject = "Sample60 Email At " + DateTime.Now.ToString(),
                TemplateName = "Solicitud_Contrato"
            };

            notification.To.Add("dbello@facturecolombia.com");

            notification.Properties.Add(new NotificationProperty("Remitente", "MARGO"));
            notification.Properties.Add(new NotificationProperty("Solicitud", "789256"));
            notification.Properties.Add(new NotificationProperty("Descripcion", "Solicitud Contrato prueba de email"));
            notification.Properties.Add(new NotificationProperty("Enlace", "http://dnndev.me/"));
            notification.Properties.Add(new NotificationProperty("Foo", "<Root><Node /></Root>"));

            //notification.Attachments.Add(new Contract.Attachment(File.Open(@"C:\Hola.docx", FileMode.Open), "Hola.docx"));

            NotificationClient.Provider.Send(notification);
        }
    }
}
