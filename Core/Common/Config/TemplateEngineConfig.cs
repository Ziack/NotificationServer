using NotificationServer.Core;
using System;
using System.Configuration;

namespace NotificationServer.Config
{
    public class TemplateEngineConfig : ConfigurationElement
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

        [ConfigurationProperty("templateResolver")]
        public ProviderConfig Resolver
        {
            get { return (ProviderConfig)this["templateResolver"]; }
            set { this["templateResolver"] = value; }
        }

        public ITemplateEngineService Build()
        {
            var resolver = Resolver.Build<ITemplateResolver>();

            var config = new TemplateServiceConfiguration
            {
                Debug = Debug,
                Resolver = resolver
            };

            var type = Type.GetType(TypeName, throwOnError: true);

            return (ITemplateEngineService)Activator.CreateInstance(type, config);
        }
    }
}