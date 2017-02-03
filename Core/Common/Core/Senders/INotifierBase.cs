using NotificationServer.Contract;
using NotificationServer.Core.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// All mailers should implement this interface.
    /// </summary>
    public interface INotifierBase : INotificationInterceptor
    {
        /// <summary>
        /// Gets or sets the default message encoding when delivering mail.
        /// </summary>
        Encoding MessageEncoding { get; }

        /// <summary>
        /// The underlying IMailSender to use for outgoing messages.
        /// </summary>
        ISender Sender { get; }

        NotificationResult Notify(Notification notification);
        NotificationResult Notify<T>(Notification notification, T model) where T : class;
    }
}
