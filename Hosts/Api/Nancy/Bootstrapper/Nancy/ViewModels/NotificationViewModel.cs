using System;
using System.Collections.Generic;

namespace NotificationServer.Nancy.ViewModels
{
    public class NotificationViewModel
    {
        [Obsolete]
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

        /// <summary>
        /// A list of tags that can be used to later search any notification
        /// </summary>
        public List<string> Tags { get; set; }

        public NotificationViewModel()
        {
            Destinations = new Dictionary<string, DestinationViewModel>();
        }
    }


    public struct Property
    {
        public string Key { get; set; }

        public object Value { get; set; }
    }

    public struct DestinationViewModel
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
