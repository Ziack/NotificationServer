using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;

namespace NotificationServer.Senders.CSScript
{
    public class CSScriptSender : ISender, IRequireRenderedBody
    {
        public string RenderedBody { get; set; }

        public void Send(Notification mail)
        {
            throw new NotImplementedException();
        }

        public void SendAsync(Notification mail, Action<Notification> callback)
        {
            throw new NotImplementedException();
        }
    }
}
