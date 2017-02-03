using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Pro.Redis;
using Microsoft.Owin;
using Nancy.Security;
using NotificationServer.Config;
using NotificationServer.Service.Entities;
using NotificationServer.Service.Repositories;
using Owin;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

[assembly: OwinStartup(typeof(NotificationServer.NancyHost.Asp.Startup))]

namespace NotificationServer.NancyHost.Asp
{
    public class Startup
    {        
        public void Configuration(IAppBuilder app)
        {
            var notificationServiceConfig = (ConfigurationManager.GetSection("notificationService") as NotificationServiceConfigSection);
            var notificationRepository = notificationServiceConfig.NotificationsRepository.Build<INotificationsRepository>();

            notificationServiceConfig.NotificationsScheduler.Build(notificationRepository).Startup(app);            

            Insight.Database.SqlInsightDbProvider.RegisterProvider();
        }
    }

}