using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Events
{
    public class NotificationRequestEvent
    {
        public long Id { get; set; }
        public string Application { get; set; }
        public string Method { get; set; }
        public string EmitterIP { get; set; }
        public string ReceiverIP { get; set; }
        public string InputMessage { get; set; }
        public string OutputMessage { get; set; }
        public string Error { get; set; }
        public string ActionCode { get; set; }
        public string ResultCode { get; set; }

        public override string ToString()
        {
            return string.Format(@"Application:{0}
Method: {1}
EmitterIP: {2}
ReceiverIP: {3}
InputMessage: {4}
OutputMessage: {5}
Error: {6}
",
    Application,
    Method,
    EmitterIP,
    ReceiverIP,
    InputMessage,
    OutputMessage,
    Error
);
        }
    }
}
