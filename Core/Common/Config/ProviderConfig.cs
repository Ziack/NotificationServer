using System;
using System.Configuration;

namespace NotificationServer.Config
{
    public class ProviderConfig : ConfigurationElement        
    {

        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }


        [ConfigurationProperty("connectionStringName", IsRequired = false)]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        public virtual T Build<T>()
            where T : class
        {            
            return (T)Build();
        }

        public virtual Object Build()            
        {
            var type = Type.GetType(TypeName, true);

            if (string.IsNullOrEmpty(ConnectionStringName))
                return Activator.CreateInstance(type); ;

            var candidateConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            var connectionString = candidateConnectionString != null ? candidateConnectionString.ConnectionString : ConnectionStringName;

            return Activator.CreateInstance(type, connectionString);
        }
    }
}