using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// This interface represents the caching layer.
    /// </summary>
    public interface ICachingProvider : IDisposable
    {
        /// <summary>
        /// Request that a given template should be cached.
        /// </summary>
        /// <param name="template">The template to be cached.</param>
        /// <param name="key">The key of the template.</param>
        void CacheTemplate(String template, String key);

        /// <summary>
        /// Try to resolve a template within the cache.
        /// </summary>
        /// <param name="key">the key of the template.</param>
        /// <param name="modelType">the model-type of the template.</param>
        /// <param name="template">the resolved template</param>
        /// <returns>true if a template was found.</returns>
        /// <remarks>
        /// Implementations MUST decide if they allow multiple model-types for the 
        /// same template key and SHOULD throw a exception when a template is requested with the wrong type!
        /// </remarks>
        bool TryRetrieveTemplate(String key, out String template);        
    }
}
