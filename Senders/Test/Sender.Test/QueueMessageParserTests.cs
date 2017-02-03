using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using NotificationServer.Contract;
using NotificationServer.Core.Runtime.Job;
using NotificationServer.Azure.Application;
using NotificationServer.Contract.Utilities;

namespace NotificationServer.Sender.Test
{
    [TestClass]
    public class QueueMessageParserTests
    {
        [TestMethod]
        public void ParserParsesQueueMessageAndExecutesAppropriateJob()
        {
            var jobs = new Dictionary<NotificationType, INotificationJob>();
            var job = new Mock<INotificationJob>();
            jobs.Add(NotificationType.Email, job.Object);
            var parser = new QueueMessageParser(jobs);

            var notification = new Notification(type: NotificationType.Email)
            {
                From = "no-reply@facturecolombia.com",
            };

            notification.To.Add("jgarcia@facturecolombia.com");

            var message = new CloudQueueMessage(notification.ToBinary());

            parser.Parse(message);

            job.Verify(j => j.ParseJobMessage(notification.ToBinary()));
            job.Verify(j => j.Execute(), Times.Once());

        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ParserThrowsAnErrorIfParticularJobDoesntExist()
        {
            var jobs = new Dictionary<NotificationType, INotificationJob>();
            var job = new Mock<INotificationJob>();
            jobs.Add(NotificationType.Email, job.Object);
            var parser = new QueueMessageParser(jobs);

            var notification = new Notification(type: NotificationType.Facebook)
            {
                From = "no-reply@facturecolombia.com",
            };

            var message = new CloudQueueMessage(notification.ToBinary());
            parser.Parse(message);
        }
    }
}
