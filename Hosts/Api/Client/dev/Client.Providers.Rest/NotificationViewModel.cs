using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Client.Providers.Rest
{
    internal class NotificationViewModel
    {
        public string ApplicationName { get; set; }

        public Guid Id { get; set; }

        public Dictionary<string, DestinationViewModel> Destinations { get; set; }

        /// <summary>
        /// A string representation of who this mail should be from.  Could be
        /// your name and email address or just an email address by itself.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The subject line of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// A collection of properties that should be used to render email.
        /// </summary>
        public List<Property> Properties { get; set; }

        public List<string> Tags { get; set; }

        public NotificationViewModel()
        {
            Destinations = new Dictionary<string, DestinationViewModel>();
            Tags = new List<string>();
        }
    }


    internal struct Property
    {
        public string Key { get; set; }

        public object Value { get; set; }
    }

    internal struct DestinationViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// A collection of addresses this email should be sent to.
        /// </summary>
        public IList<string> To { get; set; }

        /// <summary>
        /// A collection of addresses that should be CC'ed.
        /// </summary>
        public IList<string> CC { get; set; }

        /// <summary>
        /// A collection of addresses that should be BCC'ed.
        /// </summary>
        public IList<string> BCC { get; set; }

        /// <summary>
        /// A collection of addresses that should be listed in Reply-To header.
        /// </summary>
        public IList<string> ReplyTo { get; set; }
    }
}
