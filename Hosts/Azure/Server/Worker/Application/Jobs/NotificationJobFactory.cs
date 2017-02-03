using System.Collections.Generic;
using NotificationServer.Contract;
using NotificationServer.Azure.Application.Jobs.Mail;
using System.Text;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using NotificationServer.Azure.Application.Services;
using NotificationServer.Core.Runtime.Job;
using System.Net.Mail;
using NotificationServer.Senders.Mail.Senders;
using NotificationServer.Templating.Razor;
using NotificationServer.Service.SqlServer;
using NotificationServer.Core;

namespace NotificationServer.Azure.Application.Jobs
{
    public class JobFactory
    {
        public static IDictionary<NotificationType, INotificationJob> Create()
        {
            var jobs = new Dictionary<NotificationType, INotificationJob>();

            var deliveryMethod = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.DeliveryMethod");
            var deliveryFormat = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.DeliveryFormat");
            var from = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.From");
            var host = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.Host");
            var port = Convert.ToInt32(RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.Port"));
            var defaultCredentials = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.DefaultCredentials");
            var userName = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.UserName");
            var password = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.Password");
            var enableSsl = RoleEnvironment.GetConfigurationSettingValue("SmtpMailSender.EnableSsl");

            jobs.Add(NotificationType.Email, new MailJob(
                new Mailer(
                templateService: new RazorTemplateEngineService(
                    new TemplateServiceConfiguration { Resolver = new SqlServerTemplateResolver() }
                    ),
                    sender: new SmtpMailSender(client: new SmtpClient(host: host, port: port)
                    {
                        Credentials = new NetworkCredential(userName: userName, password: password),
                    }),
                    defaultFrom: from,
                    defaultMessageEncoding: Encoding.UTF8,
                    traceService: new TraceService()                    
                )
            )
            );

            return jobs;
        }
    }
}
