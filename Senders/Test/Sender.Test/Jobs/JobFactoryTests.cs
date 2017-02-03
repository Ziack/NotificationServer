using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationServer.Azure.Application.Jobs;
using NotificationServer.Azure.Application.Jobs.Mail;

namespace NotificationServer.Sender.Test.Jobs
{
    [TestClass]
    public class JobFactoryTests
    {
        [TestMethod]
        public void JobFactoryCreatesEmailJob()
        {
            var jobs = JobFactory.Create();

            Assert.IsInstanceOfType(jobs[Contract.NotificationType.Email], typeof(MailJob));
        }
    }
}
