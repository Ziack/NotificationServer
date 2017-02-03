using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// Defines the required contract for implementing template service configuration.
    /// </summary>
    public interface ITemplateServiceConfiguration
    {
        /// <summary>
        /// Gets the caching provider.
        /// </summary>
        ICachingProvider CachingProvider { get; }

        /// <summary>
        /// Gets whether the template service is operating in debug mode.
        /// </summary>
        bool Debug { get; }

        /// <summary>
        /// Gets the template resolver.
        /// </summary>        
        ITemplateResolver Resolver { get; }
    }
}
