using Nancy;
using Nancy.Security;
using Newtonsoft.Json;
using NotificationServer.Contract;
using NotificationServer.Contract.Commands;
using NotificationServer.Nancy.ViewModels;
using NotificationServer.Service;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationServer.Nancy
{
    public class NotificationsModule : NancyModule
    {
        private NotificationServerService _notificationsService;

        public NotificationsModule(NotificationServerService notificationService) : base("/Notifications")
        {
            this.RequiresAuthentication();

            _notificationsService = notificationService;
            Post["/", runAsync: true] = Notificar;
        }


        public async Task<dynamic> Notificar(dynamic context, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return null;

            //var viewModel = this.Bind<NotificationViewModel>();            

            var contentTypeRegex = new Regex("^multipart/form-data;\\s*boundary=(.*)$", RegexOptions.IgnoreCase);
            System.IO.Stream bodyStream = null;

            if (contentTypeRegex.IsMatch(this.Request.Headers.ContentType))
            {
                var boundary = contentTypeRegex.Match(this.Request.Headers.ContentType).Groups[1].Value;
                var multipart = new HttpMultipart(this.Request.Body, boundary);

                bodyStream = multipart.GetBoundaries().First().Value;
            }
            else
            {
                // Regular model binding goes here.
                bodyStream = this.Request.Body;
            }

            var jsonBody = new System.IO.StreamReader(bodyStream).ReadToEnd();
            var viewModel = JsonConvert.DeserializeObject<NotificationViewModel>(jsonBody);

            var notification = new NotifyCommand()
            {
                Id = viewModel.Id,
                ApplicationName = Context.CurrentUser.UserName,
                Destinations = viewModel.Destinations.Select(kv => new Destination
                {
                    Service = kv.Key,
                    TemplateName = kv.Value.TemplateName,
                    To = kv.Value.To,
                    CC = kv.Value.CC,
                    BCC = kv.Value.BCC,
                    ReplyTo = kv.Value.ReplyTo
                }).ToList(),
                From = viewModel.From,
                Subject = viewModel.Subject,
                Properties = viewModel.Properties.Select(p => new NotificationProperty(p.Key, p.Value)).ToList(),
                Tags = viewModel.Tags,
                Attachments = (from item in Context.Request.Files
                               select new Attachment(contentStream: item.Value, name: item.Name)).ToList()
            };

            return await _notificationsService.SendAsync(notification);
        }
    }
}