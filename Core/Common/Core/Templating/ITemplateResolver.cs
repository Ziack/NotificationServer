using System;
using System.Collections.Generic;

namespace NotificationServer.Core
{
    /// <summary>
    /// Defines the required contract for implementing a template resolver.
    /// </summary>    
    public interface ITemplateResolver
    {
        #region Methods
        /// <summary>
        /// Resolves the template content with the specified name.
        /// </summary>
        /// <param name="name">The name of the template to resolve.</param>
        /// <param name="parameters">The parameters of the template to resolve.</param>
        /// <returns>The template content.</returns>        
        string Resolve(string name, IDictionary<String, Object> parameters = null);
        #endregion
    }
}
