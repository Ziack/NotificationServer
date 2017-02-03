using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    public class NotificationSentContext
    {
        /// <summary>
        /// The generated mail message that is being sent.
        /// </summary>
        public readonly Notification Notification;

        /// <summary>
        /// The used mail template that is being sent.
        /// </summary>
        public readonly IDictionary<string, object> Parameters;

        public readonly object Response;

        public readonly String HostSender;

        public NotificationSentContext(Notification notification, IDictionary<String, Object> parameters, object response, String hostSender = null)
        {
            Notification = notification;
            Parameters = parameters;
            Response = response;
            HostSender = hostSender;
        }
    }
}
