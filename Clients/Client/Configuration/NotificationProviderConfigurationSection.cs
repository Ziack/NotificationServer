using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Client.Configuration
{
    public class NotificationProviderConfigurationSection : ConfigurationSection
    {
        public NotificationProviderConfigurationSection()
        {
            _defaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), null);
            _providers = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null);
            _properties = new ConfigurationPropertyCollection();

            _properties.Add(_providers);
            _properties.Add(_defaultProvider);
        }

        private readonly ConfigurationProperty _defaultProvider;
        [ConfigurationProperty("defaultProvider")]
        public string DefaultProvider
        {
            get { return (string)base[_defaultProvider]; }
            set { base[_defaultProvider] = value; }
        }

        private readonly ConfigurationProperty _providers;
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base[_providers]; }
        }

        private ConfigurationPropertyCollection _properties;
        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
