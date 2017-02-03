using NotificationServer.Service;
using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Config
{

    public class NotificationServiceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("notificationsRepository")]
        public ProviderConfig NotificationsRepository
        {
            get { return (ProviderConfig)this["notificationsRepository"]; }
            set { this["notificationsRepository"] = value; }
        }

        [ConfigurationProperty("settingsRepository")]
        public ProviderConfig Configurations
        {
            get { return (ProviderConfig)this["settingsRepository"]; }
            set { this["settingsRepository"] = value; }
        }

        [ConfigurationProperty("usersRepository")]
        public ProviderConfig Users
        {
            get { return (ProviderConfig)this["usersRepository"]; }
            set { this["usersRepository"] = value; }
        }

        [ConfigurationProperty("applicationsRepository")]
        public ProviderConfig Applications
        {
            get { return (ProviderConfig)this["applicationsRepository"]; }
            set { this["applicationsRepository"] = value; }
        }

        [ConfigurationProperty("scheduler")]
        public SchedulerConfig NotificationsScheduler
        {
            get { return (SchedulerConfig)this["scheduler"]; }
            set { this["scheduler"] = value; }
        }

        [ConfigurationProperty("templateEngine")]
        public TemplateEngineConfig TemplateEngine
        {
            get { return (TemplateEngineConfig)this["templateEngine"]; }
            set { this["templateEngine"] = value; }
        }

        public NotificationServerService Build()
        {
            var notifications = NotificationsRepository.Build<INotificationsRepository>();
            var configurations = Configurations.Build<IConfigurationsRepository>();
            var scheduler = NotificationsScheduler.Build(notifications);
            var templateEngine = TemplateEngine.Build();

            return new NotificationServerService(
                notifications,
                configurations,
                scheduler,
                templateEngine
            );
        }

    }
}
