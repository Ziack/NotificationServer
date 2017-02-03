using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;

namespace NotificationServer.Client.Configuration
{
    public static class ProvidersHelper
    {
        private static Type providerBaseType = typeof(ProviderBase);

        /// <summary>
        /// Instantiates the provider.
        /// </summary>
        /// <param name="providerSettings">The settings.</param>
        /// <param name="providerType">Type of the provider to be instantiated.</param>
        /// <returns></returns>
        public static ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
        {
            ProviderBase providerBase = null;
            try
            {
                string providerSetting = (providerSettings.Type == null) ? null : providerSettings.Type.Trim();
                if (string.IsNullOrEmpty(providerSetting))
                {
                    throw new ArgumentException("Provider type name is invalid");
                }
                Type c = Type.GetType(providerSetting, true, true);
                if (!providerType.IsAssignableFrom(c))
                {
                    throw new ArgumentException(String.Format("Provider must implement type {0}.", providerType.ToString()));
                }
                providerBase = (ProviderBase)Activator.CreateInstance(c);
                NameValueCollection parameters = providerSettings.Parameters;
                NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
                foreach (string parameter in parameters)
                {
                    config[parameter] = parameters[parameter];
                }
                providerBase.Initialize(providerSettings.Name, config);
            }
            catch (Exception exception)
            {
                if (exception is ConfigurationException)
                {
                    throw;
                }
                throw new ConfigurationErrorsException(exception.Message,
                    providerSettings.ElementInformation.Properties["type"].Source,
                    providerSettings.ElementInformation.Properties["type"].LineNumber);
            }
            return providerBase;
        }

        public static void InstantiateProviders(ProviderSettingsCollection providerSettings, ProviderCollection providers, Type type)
        {
            foreach (ProviderSettings settings in providerSettings)
            {
                providers.Add(InstantiateProvider(settings, type));
            }
        }
    }
}
