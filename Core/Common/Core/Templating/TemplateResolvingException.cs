using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    /// <summary>
    /// Thrown when NotificationServer cannot locate any templates for a given EmailResult
    /// </summary>
    public class TemplateResolvingException : Exception
    {
        /// <summary>
        /// Thrown when NotificationServer cannot locate any templates for a given EmailResult
        /// </summary>
        public TemplateResolvingException() { }

        /// <summary>
        /// Thrown when NotificationServer cannot locate any templates for a given EmailResult
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        public TemplateResolvingException(string message) : base(message) { }

        /// <summary>
        /// Thrown when NotificationServer cannot locate any templates for a given EmailResult
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">An inner exception which contributed to (or caused) this exception.</param>
        public TemplateResolvingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
