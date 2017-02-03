namespace NotificationServer.Core
{
    /// <summary>
    /// A simple interface that allows for reading or manipulating message
    /// messages before and after transfer.
    /// </summary>
    public interface INotificationInterceptor
    {
        /// <summary>
        /// This method is called before each message is sent
        /// </summary>
        /// <param name="context">A simple context containing the message
        /// and a boolean value that can be toggled to prevent this
        /// message from being sent.</param>
        void OnNotificationSending(ref NotificationSendingContext context);

        /// <summary>
        /// This method is called after each message is sent.
        /// </summary>
        /// <param name="notification">The message that was sent.</param>
        void OnNotificationSent(NotificationSentContext notification);
    }
}
