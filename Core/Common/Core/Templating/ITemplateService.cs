using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// Defines the required contract for implementing a template engine service.
    /// </summary>
    public interface ITemplateEngineService : IDisposable
    {
        /// <summary>
        /// Parses and returns the result of the specified string template.
        /// </summary>
        /// <param name="template">The string template XSLT.</param>
        /// <param name="model">The model instance or NULL if no model exists.</param>
        /// <returns>The string result of the template.</returns>
        string Parse(string template, object model);

        /// <summary>
        /// Parses the specified set of templates.
        /// </summary>
        /// <param name="templates">The set of string templates to parse.</param>
        /// <param name="models">
        /// The set of models or NULL if no models exist for all templates.
        /// Individual elements in this set may be NULL if no model exists for a specific template.
        /// </param>
        /// <param name="parallel">Flag to determine whether parsing in templates.</param>
        /// <returns>The set of parsed template results.</returns>
        IEnumerable<string> ParseMany(IEnumerable<string> templates, IEnumerable<object> models, bool parallel);

    }
}
