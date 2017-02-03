using NotificationServer.Contract.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract.Commands
{
    public class NotifyCommand
    {
        /// <summary>
        /// The name of the application for which this message will be sent.
        /// It determines what services are available and what is their configuration.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Services to which this message will be sent.
        /// </summary>
        public List<Destination> Destinations { get; set; }

        /// <summary>
        /// A string representation of who this mail should be from.  Could be
        /// your name and email address or just an email address by itself.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The subject line of the email.
        /// </summary>
        public string Subject { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Any attachments you wish to add.  The key of this collection is what
        /// the file should be named.  The value is should represent the binary bytes
        /// of the file.
        /// </summary>
        /// <example>
        /// Attachments["picture.jpg"] = File.ReadAllBytes(@"C:\picture.jpg");
        /// </example>        
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// A list of tags that can be used to later search any notification
        /// </summary>
        public List<string> Tags { get; set; }
                
        public string Response { get; set; }

        public string urlSender { get; set; }

        public NotifyCommand() : this(null)
        {

        }

        public NotifyCommand(string applicationName)
        {
            ApplicationName = applicationName;
            Attachments = new List<Attachment>();
            Destinations = new List<Destination>();
            Properties = new List<NotificationProperty>();
            Tags = new List<string>();
        }

        /// <summary>
        /// A collection of properties that should be used to render email.
        /// </summary>
        public IList<NotificationProperty> Properties { get; set; }

        public Notification GetNotificationFor(string service)
        {
            var destination = Destinations.First(d => d.Service == service);
            return new Notification
            {
                From = From,
                Subject = Subject,
                TemplateName = destination.TemplateName,
                PartitionKey = Id
            }
                .WithTo(destination.To)
                .WithCC(destination.CC)
                .WithBCC(destination.BCC)
                .WithReplyTo(destination.ReplyTo)
                .WithProperties(Properties)
                .WithAttachments(Attachments)
                .WithTags(Tags);
        }

        public override string ToString()
        {
            return string.Format(
@"ApplicationName:{0}
Destinations: 
    - {1}
From: {2}
Subject: {3}
Attachments: 
    - {4}
Tags: {5}
PartitionKey: {6}",
            ApplicationName,
            string.Join("\n    - ", Destinations),
            From,
            Subject,
            string.Join("\n    - ", Attachments),
            string.Join(", ", Tags),
            Id.ToString()
);
        }
    }

    public class Destination
    {
        /// <summary>
        /// 
        /// </summary>
        public string TemplateName { get; set; }


        public string Service { get; set; }

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


        public Destination()
        {
            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
            ReplyTo = new List<string>();
        }

        public override string ToString()
        {
            return string.Format(@"TemplateName: {0}
Service: {1}
To: {2}
CC: {3}
BCC: {4}
ReplyTo: {5}
", TemplateName, Service,
string.Join(", ", To),
string.Join(", ", CC),
string.Join(", ", BCC),
string.Join(", ", ReplyTo)
);
        }

    }
}
