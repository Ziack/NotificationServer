using NotificationServer.Core;
using System.Linq;

namespace NotificationServer.Interceptors.CoreInterceptors
{
    /// <summary>
    /// Rewrite tokens or placeholders that map to any notification fields for Subject or Body or any other field
    /// </summary>
    public class OptionsTokenReplacingInterceptor : INotificationInterceptor
    {
        public void OnNotificationSending(ref NotificationSendingContext context)
        {
            var options = context.Parameters.ToList();
            var overrides = context.Notification.Properties;

            foreach (var @override in overrides)
                foreach (var option in options)
                {
                    var optionToken = "{" + @override.Key + "}";

                    if (option.Key == @override.Key)
                        context.Parameters[option.Key] = @override.Value;
                    else if (option.Value is string && (option.Value as string).Contains(optionToken))
                        context.Parameters[option.Key] = (option.Value as string).Replace(optionToken, @override.Value as string);
                }
        }

        public void OnNotificationSent(NotificationSentContext notification) { }
    }
}
