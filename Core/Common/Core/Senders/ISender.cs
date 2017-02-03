using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    public interface ISender
    {
        /// <summary>
        /// Sends mail synchronously.
        /// </summary>
        /// <param name="mail">The mail message you wish to send.</param>
        void Send(Notification mail);

        /// <summary>
        /// Sends mail asynchronously.
        /// </summary>
        /// <param name="mail">The mail message you wish to send.</param>
        /// <param name="callback">The callback method that will be fired when sending is complete.</param>
        void SendAsync(Notification mail, Action<Notification> callback);
    }
}
