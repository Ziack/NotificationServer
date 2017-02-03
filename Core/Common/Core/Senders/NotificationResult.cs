using NotificationServer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Senders
{
    /// <summary>
    /// An container for MailMessage with the appropriate body rendered by Razor.
    /// </summary>
    public class NotificationResult
    {
        private readonly INotificationInterceptor _interceptor;
        private readonly DeliveryHelper _deliveryHelper;
        private readonly ITemplateEngineService _templateService;
        private readonly string _templateName;

        /// <summary>
        /// The underlying MailMessage object that was passed to this object's constructor.
        /// </summary>
        public readonly Notification Notification;

        /// <summary>
        /// The IMailSender instance that is used to deliver mail.
        /// </summary>
        public readonly ISender Sender;

        /// <summary>
        /// The default encoding used to send a message.
        /// </summary>
        public readonly Encoding MessageEncoding;


        /// <summary>
        /// Creates a new EmailResult.  You must call Compile() before this result
        /// can be successfully delivered.
        /// </summary>
        /// <param name="interceptor">The IMailInterceptor that we will call when delivering mail.</param>
        /// <param name="sender">The IMailSender that we will use to send mail.</param>
        /// <param name="notification">The notification message who's body needs populating.</param>
        /// <param name="viewName">The view to use when rendering the message body.</param>
        /// <param name="messageEncoding">The encoding to use when rendering a message.</param>
        /// <param name="viewPath">The path where we should search for the view.</param>
        /// <param name="templateService">The template service defining a ITemplateResolver and a TemplateBase</param>
        /// <param name="viewBag">The viewBag is a dynamic object that can transfer data to the view</param>
        public NotificationResult(INotificationInterceptor interceptor, ISender sender, Notification notification, string templateName, Encoding messageEncoding, ITemplateEngineService templateService)
        {
            if (interceptor == null)
                throw new ArgumentNullException("interceptor");

            if (sender == null)
                throw new ArgumentNullException("sender");

            if (notification == null)
                throw new ArgumentNullException("notification");

            if (templateService == null)
                throw new ArgumentNullException("templateService");

            _interceptor = interceptor;
            Sender = sender;
            Notification = notification;
            MessageEncoding = messageEncoding;
            _templateName = templateName;
            _deliveryHelper = new DeliveryHelper(sender, _interceptor);

            _templateService = templateService;
        }

        public NotificationResult(INotificationInterceptor interceptor, ISender sender, Notification notification, Encoding messageEncoding, ITemplateEngineService templateService)
            : this(interceptor, sender, notification, notification.TemplateName, messageEncoding, templateService)
        {
        }

        /// <summary>
        /// Sends your message.  This call will block while the message is being sent. (not recommended)
        /// </summary>
        public void Deliver(IDictionary<string, object> parameters = null)
        {
            parameters = parameters ?? new Dictionary<string, object>();
            parameters.Add(Globals.TEMPLATE_NAME_PARAMETER_NAME, _templateName);

            _deliveryHelper.Deliver(async: false, notification: Notification, parameters: parameters);
        }

        /// <summary>
        /// Sends your message asynchronously.  This method does not block.  If you need to know
        /// when the message has been sent, then override the OnMailSent method in MailerBase which
        /// will not fire until the asyonchronous send operation is complete.
        /// </summary>
        public void DeliverAsync(IDictionary<string, object> parameters = null)
        {
            parameters = parameters ?? new Dictionary<string, object>();
            parameters.Add(Globals.TEMPLATE_NAME_PARAMETER_NAME, _templateName);

            _deliveryHelper.Deliver(async: true, notification: Notification, parameters: parameters);
        }

        /// <summary>
        /// Compiles the email body using the specified Razor view and model.
        /// </summary>
        public string Compile<T>(T model, bool trimBody)
        {
            try
            {
                var body = _templateService.Parse(_templateName, model);
                if (trimBody)
                    body = body.Trim();


                return body;
                //TOOD: Resolve
                //var altView = AlternateView.CreateAlternateViewFromString(body, MessageEncoding, MediaTypeNames.Text.Plain);
                //Mail.AlternateViews.Add(altView);
            }
            catch (TemplateResolvingException e)
            {
                throw new NoTemplatesFoundException(string.Format("Could not find any template named {0}.", _templateName), e);
            }
        }
    }
}
