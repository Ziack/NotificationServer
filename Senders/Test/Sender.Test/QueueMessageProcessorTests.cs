using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Queue;
using Moq;
using NotificationServer.Core.Runtime.Services;
using NotificationServer.Core.Runtime.Queue;
using NotificationServer.Azure.Application;

namespace NotificationServer.Sender.Test
{
    [TestClass]
    public class QueueMessageProcessorTests
    {
        [TestMethod]
        public void ProcessorGetsMessageFromQueue()
        {
            // arrange
            var message = new CloudQueueMessage("email;test");
            var queue = new Mock<IQueue>();
            queue.Setup(q => q.GetMessage()).Returns(message);

            var sleepService = new Mock<ISleepService>();
            var traceService = new Mock<ITraceService>();
            var parser = new Mock<IQueueMessageParser>();

            var processor = new QueueMessageProcessor(sleepService.Object, queue.Object, traceService.Object, parser.Object);

            // act
            processor.Run(message);

            // assert
            queue.Verify(q => q.GetMessage(), Times.Once());
            parser.Verify(p => p.Parse(message), Times.Once());
            queue.Verify(q => q.DeleteMessage(message), Times.Once());
        }

        [TestMethod]
        public void ProcessorSleepsForOneMinuteIfTheresNoMessage()
        {
            var queue = new Mock<IQueue>();
            CloudQueueMessage message = null;
            queue.Setup(q => q.GetMessage()).Returns(message);

            var sleepService = new Mock<ISleepService>();
            var traceService = new Mock<ITraceService>();
            var parser = new Mock<IQueueMessageParser>();

            var processor = new QueueMessageProcessor(sleepService.Object, queue.Object, traceService.Object, parser.Object);

            processor.Run(message);

            queue.Verify(q => q.GetMessage(), Times.Once());
            sleepService.Verify(s => s.Sleep(60000), Times.Once());
        }

        [TestMethod]
        public void ProcessorTracesThrownErrorAndSleepsForOneMinute()
        {
            var queue = new Mock<IQueue>();
            var message = new CloudQueueMessage("job;type");
            queue.Setup(q => q.GetMessage()).Returns(message);

            var sleepService = new Mock<ISleepService>();
            var traceService = new Mock<ITraceService>();
            var parser = new Mock<IQueueMessageParser>();
            parser.Setup(p => p.Parse(message)).Throws<NotImplementedException>();

            var processor = new QueueMessageProcessor(sleepService.Object, queue.Object, traceService.Object, parser.Object);

            processor.Run(message);

            traceService.Verify(t => t.TraceError(It.IsAny<string>()), Times.Once());
            sleepService.Verify(s => s.Sleep(60000), Times.Once());
        }
    }
}
