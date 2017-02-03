using NotificationServer.Core;
using NotificationServer.Service.Repositories;
using System;
using System.Configuration;

namespace NotificationServer.Config
{
    public class SchedulerConfig : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("debug", DefaultValue = "true", IsRequired = false)]
        public bool Debug
        {
            get { return (bool)this["debug"]; }
            set { this["debug"] = value; }
        }

        [ConfigurationProperty("storage")]
        public ProviderConfig Storage
        {
            get { return (ProviderConfig)this["storage"]; }
            set { this["storage"] = value; }
        }

        public INotificationsScheduler Build(INotificationsRepository notificationRepository)
        {
            var storage = Storage.Build();

            var type = Type.GetType(TypeName, throwOnError: true);

            return (INotificationsScheduler)Activator.CreateInstance(type, storage, notificationRepository);
        }
    }
}