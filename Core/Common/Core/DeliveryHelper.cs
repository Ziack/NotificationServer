using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// Some helpers used to dilver mail.  Reduces the need to repeat code.
    /// </summary>
    public class DeliveryHelper
    {
        private ISender _sender;
        private INotificationInterceptor _interceptor;

        /// <summary>
        /// Creates a new dilvery helper to be used for sending messages.
        /// </summary>
        /// <param name="sender">The sender to use when delivering mail.</param>
        /// <param name="interceptor">The interceptor to report with while delivering mail.</param>
        public DeliveryHelper(ISender sender, INotificationInterceptor interceptor)
        {
            if (interceptor == null)
                throw new ArgumentNullException("interceptor");

            if (sender == null)
                throw new ArgumentNullException("sender");

            _sender = sender;
            _interceptor = interceptor;
        }

        /// <summary>
        /// Sends the given email using the given
        /// </summary>
        /// <param name="async">Whether or not to use asynchronous delivery.</param>
        /// <param name="notification">The mail message to send.</param>
        public void Deliver(bool async, Notification notification, IDictionary<string, object> parameters)
        {
            if (notification == null)
                throw new ArgumentNullException("notification");

            var mailContext = new NotificationSendingContext(notification: notification, parameters: parameters, sender: _sender);
            _interceptor.OnNotificationSending(ref mailContext);

            if (mailContext.Cancel)
                return;

            if (async)
            {
                _sender.SendAsync(notification, n => NotifyToInterceptor(n, parameters));
                return;
            }

            _sender.Send(notification);
            NotifyToInterceptor(notification, parameters);
        }

        private void NotifyToInterceptor(Notification notification, IDictionary<string, object> parameters)
        {
            var response = _sender is IYieldResponse ? (_sender as IYieldResponse).Response : default(object);
            var urlSender = _sender is IYieldResponse ? (_sender as IYieldResponse).UrlSender : default(String);
            _interceptor.OnNotificationSent(new NotificationSentContext(notification, parameters, response, urlSender));
        }
    }
}
