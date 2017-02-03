using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;
using TweetSharp;

namespace NotificationServer.Senders.Twitter
{
    public class TwitterSender : ISender, IRequireRenderedBody
    {
        private TwitterSetting setting; 
        public TwitterSender() { }

        public TwitterSender(IDictionary<string, object> options)
        {
            setting = new TwitterSetting
            {
                AccessToken = (options.ContainsKey("AccessToken")) ? options["AccessToken"].ToString() : "",
                AccessTokenSecret = options.ContainsKey("AccessTokenSecret") ? options["AccessTokenSecret"].ToString() : "",
                ConsumerKey = options.ContainsKey("ConsumerKey") ? options["ConsumerKey"].ToString() : "",
                ConsumerSecret = options.ContainsKey("ConsumerSecret") ? options["ConsumerSecret"].ToString() : ""
            };
        }
        public string RenderedBody{get; set;}

        public void Send(Notification notificacion)
        {
            var service = new TwitterService(setting.ConsumerKey, setting.ConsumerSecret);
            service.AuthenticateWith(setting.AccessToken, setting.AccessTokenSecret);
            var profilers = service.GetUserProfileFor(new GetUserProfileForOptions { ScreenName = notificacion.To[0]});
            if (profilers != null)
            {
                var rest = service.SendDirectMessage(new SendDirectMessageOptions
                {
                    ScreenName = notificacion.To[0],
                    Text = RenderedBody
                });
            }
        }

        public void SendAsync(Notification notification, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(notification));
            task.ContinueWith(t => callback(notification));
        }

    }

}
