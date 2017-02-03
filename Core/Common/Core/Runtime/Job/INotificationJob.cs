using System;

namespace NotificationServer.Core.Runtime.Job
{
    public interface INotificationJob
    {
        void ParseJobMessage(Byte[] message);

        void Execute();
    }
}
