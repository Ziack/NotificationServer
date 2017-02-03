using NotificationServer.Service.Entities;
using Owin;

namespace NotificationServer.Service.Repositories
{
    public interface INotificationsScheduler
    {
        void Startup(IAppBuilder app);

        void Add(NotificationRunner spec);
    }
}