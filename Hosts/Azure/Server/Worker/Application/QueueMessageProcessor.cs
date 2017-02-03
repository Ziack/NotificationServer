using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NotificationServer.Contract;
using NotificationServer.Core.Runtime.Queue;
using NotificationServer.Core.Runtime.Services;
using NotificationServer.Core.Utilities;
using NotificationServer.Azure.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Azure.Application
{
    public class QueueMessageProcessor
    {
        private readonly ISleepService _sleepService;
        private readonly IQueue _queue;
        private readonly ITraceService _traceService;
        private readonly IQueueMessageParser _messageParser;

        public QueueMessageProcessor(ISleepService sleepService, IQueue queue, ITraceService traceService, IQueueMessageParser messageParser)
        {
            _sleepService = sleepService;
            _queue = queue;
            _traceService = traceService;
            _messageParser = messageParser;
        }

        public void Run(CloudQueueMessage msg)
        {
            try
            {
                bool messageFound = false;

                msg = _queue.GetMessage();
                if (msg != null)
                {
                    if (msg.DequeueCount > 5)
                    {
                        var notification = msg.FromMessage<Notification>();
                        _traceService.TraceError(String.Format("Deleting poison job message:    message {0}.", JsonConvert.SerializeObject(notification)));
                        _queue.DeleteMessage(msg);
                    }
                    else
                    {
                        _messageParser.Parse(msg);
                        _queue.DeleteMessage(msg);
                        messageFound = true;
                    }
                }

                if (messageFound == false)
                {
                    _sleepService.Sleep(1000 * 60);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (ex.InnerException != null)
                {
                    err += " Inner Exception: " + ex.InnerException.Message;
                }
                if (msg != null)
                {
                    //err += " Last queue message retrieved: " + msg.AsString;
                }
                _traceService.TraceError(err);
                _sleepService.Sleep(1000 * 60);
            }
        }
    }
}
