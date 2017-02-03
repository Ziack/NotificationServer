using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using NotificationServer.Contract;
using NotificationServer.Contract.Utilities;
using System.Configuration;
using NotificationServer.Client.Configuration;
using NotificationServer.Contract.Commands;

namespace NotificationServer.Client
{
    public static class NotificationClient
    {
        private static bool _isInitialized = false;

        private static NotificationProvider _provider;
        public static NotificationProvider Provider
        {
            get
            {
                Initialize();
                return _provider;
            }
        }

        private static NotificationProviderCollection _providers;
        public static NotificationProviderCollection Providers
        {
            get
            {
                Initialize();
                return _providers;
            }
        }

        private static void Initialize()
        {
            NotificationProviderConfigurationSection dataConfig = null;

            if (!_isInitialized)
            {
                // get the configuration section for the feature
                dataConfig = (NotificationProviderConfigurationSection)ConfigurationManager.GetSection("notification");

                if (dataConfig == null)
                {
                    throw new ConfigurationErrorsException("Data is not configured to be used with this application");
                }

                _providers = new NotificationProviderCollection();

                // use the ProvidersHelper class to call Initialize() on each provider
                ProvidersHelper.InstantiateProviders(dataConfig.Providers, _providers, typeof(NotificationProvider));

                // set a reference to the default provider
                _provider = _providers[dataConfig.DefaultProvider] as NotificationProvider;

                _isInitialized = true;
            }
        }

        [Obsolete]
        public static void Send(Notification notification)
        {
            Initialize();
            if (_provider != null)
            {
                _provider.Send(notification: notification);
            }
        }

        public static void Send(NotifyCommand notification)
        {
            Initialize();
            if (_provider != null)
            {
                _provider.Send(notification: notification);
            }
        }
    }
}
