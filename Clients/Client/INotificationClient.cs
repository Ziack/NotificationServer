using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;

namespace NotificationServer.Client
{
    public interface INotificationClient
    {
        void Send(Notification notification);
    }
}
