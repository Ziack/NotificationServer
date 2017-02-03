using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.ServiceBus;
using Rhino.ServiceBus.Hosting;
using NotificationServer.Contract;
using Rhino.ServiceBus.Msmq;
using System.Collections;

namespace NotificationServer.Windows.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PrepareQueues.Prepare("msmq://localhost/Notification.Client", QueueType.Standard);

            var host = new DefaultHost();
            host.Start<ClientBootStrapper>();
            var bus = host.Bus as IServiceBus;

            var notification = new Notification(NotificationType.Email)
            {
                From = "noreply@facturecolombia.com",
                Subject = "Demo",
                TemplateName = "Solicitud_Contrato"
            };

            notification.To.Add("jagarcia@facturecolombia.com");

            notification.Properties.Add(new NotificationProperty("Remitente", "MARGO"));
            notification.Properties.Add(new NotificationProperty("Solicitud", "789256"));
            notification.Properties.Add(new NotificationProperty("Descripcion", "Solicitud Contrato prueba de email"));
            notification.Properties.Add(new NotificationProperty("Enlace", "http://dnndev.me/"));

            Console.ReadLine();
            bus.Send(notification);
            Console.ReadLine();
        }
    }
}
