using NotificationServer.Contract;
using NotificationServer.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Client.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NotificationProvider : ProviderBase
    {
        protected string ConnectionStringName { get; set; }

        protected string QueueName { get; set; }

        [Obsolete("This method will be removed, however, for now it is the only way to use the Azure blobs and Windows service interfaces. In order to use the REST interface, use the Send(NotifyCommand) overload of this method on the REST provider.")]
        public virtual void Send(Notification notification)
        {
            throw new NotImplementedException("This provider does not implement this overload of Send, use the Send(NotifyCommand) overload instead.");
        }


        /// <summary>
        /// Sends a notification to the notification server.
        /// </summary>
        /// <param name="notification">The notification to be sent.</param>
        /// <returns>The GUID of the notification for future tracking.</returns>
        public abstract Guid Send(NotifyCommand notification);

        /// <summary>
        /// Sends a provisioning to the notification server.
        /// </summary>
        /// <param name="provisioning">The Provisioning to be sent.</param>
        /// <returns>The GUID of the provisioning for future tracking.</returns>
        public abstract Guid Provisioning(ProvisioningCommand provisioning);



        public override void Initialize(string name, NameValueCollection config)
        {
            this.ConnectionStringName = config["connectionStringName"];
            this.QueueName = config["queueName"];

            if (string.IsNullOrEmpty(this.ConnectionStringName))
                throw new ConfigurationErrorsException("connectionStringName");

            base.Initialize(name, config);
        }
    }
}
