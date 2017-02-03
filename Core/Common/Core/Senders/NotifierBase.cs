using log4net;
using NotificationServer.Contract;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NotificationServer.Core.Senders
{
    /// <summary>
    /// This is a standalone MailerBase that relies on RazorEngine to generate emails.
    /// </summary>
    public abstract class NotifierBase : INotifierBase
    {
        private static readonly ILog _log = LogManager.GetLogger(Globals.LOG4NET_WORKFLOW_SERVER_API_SOURCE);

        /// <summary>
        /// Gets or sets the default message encoding when delivering mail.
        /// </summary>
        public Encoding MessageEncoding { get; protected set; }

        /// <summary>
        /// The underlying IMailSender to use for outgoing messages.
        /// </summary>
        public ISender Sender { get; set; }

        public ITemplateEngineService TemplateService { get; set; }

        /// <summary>
        /// This method is called after each mail is sent.
        /// </summary>
        /// <param name="mail">The mail that was sent.</param>
        protected virtual void OnNotificationSent(NotificationSentContext mail) { }

        /// <summary>
        /// This method is called before each mail is sent
        /// </summary>
        /// <param name="context">A simple context containing the mail
        /// and a boolean value that can be toggled to prevent this
        /// mail from being sent.</param>
        protected virtual void OnNotificationSending(NotificationSendingContext context) { }

        void INotificationInterceptor.OnNotificationSending(ref NotificationSendingContext context)
        {
            OnNotificationSending(context);
        }

        void INotificationInterceptor.OnNotificationSent(NotificationSentContext notification)
        {
            OnNotificationSent(notification);
        }
        /// <summary>
        /// Initializes MailerBase using the default SmtpMailSender and system Encoding.
        /// </summary>
        /// <param name="templateResolver"></param>
        /// <param name="templateService"></param>        
        /// <param name="mailSender">The underlying mail sender to use for delivering mail.</param>
        /// <param name="defaultMessageEncoding">The default encoding to use when generating a mail message.</param>        
        protected NotifierBase(ITemplateEngineService templateService, ISender mailSender = null, Encoding defaultMessageEncoding = null)
        {
            Sender = mailSender;
            MessageEncoding = defaultMessageEncoding ?? Encoding.UTF8;
            TemplateService = templateService;
        }

        public NotifierBase() { }


        public virtual NotificationResult Notify<T>(Notification notification, T model) where T : class
        {
            _log.Info("Generating Email.");
            var result = new NotificationResult(this, Sender, notification, MessageEncoding, TemplateService);

            if (Sender is IRequireRenderedBody)
            {
                _log.Info("Compiling Email.");
                (Sender as IRequireRenderedBody).RenderedBody = result.Compile(model, trimBody: true);
            }
            else
                _log.Info("Email does not require compiling.");

            return result;
        }


        public virtual NotificationResult Notify(Notification notification)
        {
            var emaiData = new NotificationData(
                from: notification.From,
                subject: notification.Subject,
                to: notification.To,
                CC: notification.CC,
                BCC: notification.BCC,
                replyTo: notification.ReplyTo,
                properties: notification.Properties.ToDictionary(k => k.Key, v => v.Value)
            );

            return Notify(notification, emaiData);
        }
    }
}
