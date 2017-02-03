using NotificationServer.Contract;
using NotificationServer.Core;
using NotificationServer.Core.Runtime.Job;
using NotificationServer.Senders.Mail.Senders;
using NotificationServer.Service.SqlServer;
using NotificationServer.Templating.Razor;
using NotificationServer.Windows.Application.Jobs.Mail;
using NotificationServer.Windows.Application.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NotificationServer.Windows.Application.Jobs
{
    public class JobFactory
    {
        public static IDictionary<NotificationType, INotificationJob> Create()
        {
            var jobs = new Dictionary<NotificationType, INotificationJob>();

            var deliveryMethod = ConfigurationManager.AppSettings["SmtpMailSender.DeliveryMethod"];
            var deliveryFormat = ConfigurationManager.AppSettings["SmtpMailSender.DeliveryFormat"];
            var from = ConfigurationManager.AppSettings["SmtpMailSender.From"];
            var host = ConfigurationManager.AppSettings["SmtpMailSender.Host"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpMailSender.Port"]);
            var defaultCredentials = ConfigurationManager.AppSettings["SmtpMailSender.DefaultCredentials"];
            var userName = ConfigurationManager.AppSettings["SmtpMailSender.UserName"];
            var password = ConfigurationManager.AppSettings["SmtpMailSender.Password"];
            var enableSsl = ConfigurationManager.AppSettings["SmtpMailSender.EnableSsl"];

            jobs.Add(NotificationType.Email, new MailJob(
                new Mailer(
                templateService: new RazorTemplateEngineService(
                    new TemplateServiceConfiguration { Resolver = new SqlServerTemplateResolver() }                        
                    ),
                    mailSender: new SmtpMailSender(client: new SmtpClient(host: host, port: port)
                    {
                        Credentials = new NetworkCredential(userName: userName, password: password),
                    }),
                    defaultMessageEncoding: Encoding.UTF8,
                    traceService: new TraceService()
                )
            )
            );

            return jobs;
        }
    }
}
