using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationServer.Core;
using NotificationServer.Core.Senders;
using System.Net.Mail;
using NotificationServer.Contract;
using NotificationServer.Provider.PuertoBahia.TemplateServices;
using NotificationServer.Core.Runtime.Job;
using NotificationServer.Azure.Application.Jobs.Mail;
using NotificationServer.Contract.Utilities;
using NotificationServer.Provider.PuertoBahia.TemplateResolvers;
using NotificationServer.Provider.PuertoBahia.ServiceConfiguration;
using NotificationServer.Provider.PuertoBahia.CachingProvider;
using System.Collections.Generic;

namespace NotificationServer.Sender.Test.Jobs.Mail
{
    [TestClass]
    public class MailJobTests
    {
        [TestMethod]
        public void MailJobParsesEmailQueueMessageAndSendsEmail()
        {
            var sender = new Mock<ISender>();
            var mailInterceptor = new Mock<INotificationInterceptor>();
            var templateEngine = new Mock<XSLTTemplateEngineService>(new PuertoBahiaTemplateServiceConfiguration(
                resolver: new PuertoBahiaTemplateResolver(),
                cachingProvider: new PuertoBahiaCachingProvider()
            ));

            var notification = new Notification();
            notification.From = "no-reply@facturecolombia.com";
            notification.To.Add("jgarcia@facturecolombia.com");

            var emailResult = new NotificationResult(mailInterceptor.Object, sender.Object, notification, null, templateEngine.Object);

            var mailer = new Mock<INotifier>();
            mailer.Setup(m => m.Run(
                notification.TemplateName,
                notification.From,
                notification.Subject,
                notification.To,
                new List<string>(),
                new List<string>(),
                new List<string>(),
                new Dictionary<string, object>(),
                new Core.AttachmentCollection()
            )).Returns(emailResult);

            var mailJob = new MailJob(mailer.Object);


            mailJob.ParseJobMessage(notification.ToBinary());
            mailJob.Execute();

            mailer.Verify(m => m.Run(
                notification.TemplateName,
                notification.From,
                notification.Subject,
                notification.To,
                new List<string>(),
                new List<string>(),
                new List<string>(),
                new Dictionary<string, object>(),
                new Core.AttachmentCollection()
            ), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void MailJobThrowsAnErrorIfMailTypeIsUnknown()
        {
            var mailer = new Mock<INotifier>();
            var mailJob = new MailJob(mailer.Object);

            var notification = new Notification(type: NotificationType.Email)
            {
                From = "no-reply@facturecolombia.com",
            };

            notification.To.Add("jgarcia@facturecolombia.com");

            mailJob.ParseJobMessage(notification.ToBinary());

            mailJob.Execute();
        }
    }
}
