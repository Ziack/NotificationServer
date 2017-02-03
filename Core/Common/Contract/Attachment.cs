using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract.Utilities;

namespace NotificationServer.Contract
{
    [Serializable]
    public class Attachment
    {
        /// <summary>
        /// Gets the content stream of this attachment. A System.IO.Stream.The content stream of this attahcment.
        /// </summary>
        [field: NonSerialized]
        public Stream ContentStream
        {
            get
            {
                return new MemoryStream(Convert.FromBase64String(this.Content));
            }
        }

        public String Content { get; private set; }

        // Summary:
        //     
        //
        // Returns:
        //     A System.Net.Mime.ContentDisposition that provides the presentation information
        //     for this attachment.

        /// <summary>
        /// Gets the MIME content disposition for this attachment.
        /// Return A System.Net.Mime.ContentDisposition that provides the presentation information for this attachment.
        /// </summary>
        public ContentDisposition ContentDisposition { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets or sets the MIME content type name value in the content type associated with this attachment.
        /// A System.String that contains the value for the content type name represented by the System.Net.Mime.ContentType.Name property.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The value specified for a set operation is null.</exception>
        /// <exception cref="System.ArgumentException">The value specified for a set operation is System.String.Empty ("").</exception>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the encoding for the System.Net.Mail.AttachmentSystem.Net.Mail.Attachment.Name.
        /// An System.Text.Encoding value that specifies the type of name encoding. The
        /// default value is determined from the name of the attachment.
        /// </summary>
        public Encoding NameEncoding { get; set; }

        /// <summary>
        /// Initializes a new instance of the System.Net.Mail.Attachment class with the specified stream and name.
        /// </summary>
        /// <param name="contentStream">A readable System.IO.Stream that contains the content for this attachment.</param>
        /// <param name="name">A System.String that contains the value for the System.Net.Mime.ContentType.Name property of the System.Net.Mime.ContentType associated with this attachment. This value can be null.</param>
        /// <exception cref="System.ArgumentNullException">contentStream is null.</exception>
        public Attachment(Stream contentStream, string name)
        {
            this.Content = Convert.ToBase64String(contentStream.ReadAllBytes());
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
