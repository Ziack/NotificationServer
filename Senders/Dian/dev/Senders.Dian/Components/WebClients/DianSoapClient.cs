using Microsoft.Web.Services3;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NotificationServer.Senders.Dian.Components.WebClients
{
    public class DianSoapClient
    {
        #region Constants
        // namespaces
        protected const string NS_WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        protected const string NS_WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        /* 
         * WARNING: 'application/soap+xml' content-type raises a huge error exception pile.
         *
         * INVALID: public const string DIAN_CONTENT_TYPE = "application/soap+xml; charset=utf-8";
         * Excepction (Excerpt):
         * JBoss Web/7.4.8.Final-redhat-4 - JBWEB000064: Error report</title><style><!--H1 {font-family:Tahoma,Arial,sans-serif;color:white;background-color:#525D76;font-size:22px;} H2 {font-family:Tahoma,Arial,sans-serif;color:white;background-color:#525D76;font-size:16px;} H3 {font-family:Tahoma,Arial,sans-serif;color:white;background-color:#525D76;font-size:14px;} BODY {font-family:Tahoma,Arial,sans-serif;color:black;background-color:white;} B {font-family:Tahoma,Arial,sans-serif;color:white;background-color:#525D76;} P {font-family:Tahoma,Arial,sans-serif;background:white;color:black;font-size:12px;}A {color : black;}A.name {color : black;}HR {color : #525D76;}--></style> </head><body><h1>JBWEB000065: HTTP Status 500 - Request processing failed; nested exception is org.springframework.ws.soap.SoapMessageCreationException: Could not create message from InputStream: Unable to internalize message; nested exception is com.sun.xml.messaging.saaj.SOAPExceptionImpl: Unable to internalize message</h1><HR size="1" noshade="noshade"><p><b>JBWEB000309: type</b> JBWEB000066: Exception report</p><p><b>JBWEB000068: message</b> <u>Request processing failed; nested exception is org.springframework.ws.soap.SoapMessageCreationException: Could not create message from InputStream: Unable to internalize message; nested exception is com.sun.xml.messaging.saaj.SOAPExceptionImpl: Unable to internalize message</u></p><p><b>JBWEB000069: description</b> <u>JBWEB000145: The server encountered an internal error that prevented it from fulfilling this request.</u></p><p><b>JBWEB000070: exception</b> <pre>org.springframework.web.util.NestedServletException: Request processing failed; nested exception is org.springframework.ws.soap.SoapMessageCreationException: Could not create message from InputStream: Unable to internalize message; nested exception is com.sun.xml.messaging.saaj.SOAPExceptionImpl: Unable to internalize message
         * org.springframework.web.servlet.FrameworkServlet.processRequest(FrameworkServlet.java:973)
         * org.springframework.web.servlet.FrameworkServlet.doPost(FrameworkServlet.java:863)
         * javax.servlet.http.HttpServlet.service(HttpServlet.java:754)
         * org.springframework.web.servlet.FrameworkServlet.service(FrameworkServlet.java:837)
         * javax.servlet.http.HttpServlet.service(HttpServlet.java:847)
         */
        public const string DIAN_CONTENT_TYPE = "text/xml; charset=utf-8";

        #endregion
        #region Properties
        public string Url { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string NOnce { get; private set; }
        public DateTime Created { get; private set; }
        public SoapEnvelope RequestEnvelope { get; private set; }
        public SoapEnvelope ResponseEnvelope { get; private set; }
        #endregion

        #region Constructor
        public DianSoapClient(string url, string username, string password)
        {
            this.Url = url;
            this.Username = username;
            this.Password = password;
        }
        #endregion

        #region Public
        public string Send(string actionName, string actionNamespace, Action<XmlElement, string> addParameters)
        {
            // create soap envelope
            ResponseEnvelope = null;
            RequestEnvelope = new SoapEnvelope();
            RequestEnvelope.DocumentElement.OwnerDocument.PreserveWhitespace = true;
            EnvelopeAddHeader();
            EnvelopeAddBody(actionName, actionNamespace, addParameters);

            // create request
            var request = WebRequest.CreateHttp(Url);
            request.Headers.Add("SOAPAction", "");
            request.ContentType = DIAN_CONTENT_TYPE;
            request.Method = "POST";
            request.Accept = "Multipart/Related";

            // send request
            using (var requestStream = request.GetRequestStream()) { RequestEnvelope.Save(requestStream); }


            // get and return response
            return ParseReponse(request);
        }
        #endregion Public

        #region Parsing Response
        private string ParseReponse(HttpWebRequest request)
        {
            // get response object
            HttpWebResponse response;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            // process multi-part message
            var responseText = ParseReponseMultiPart(response);

            // parse body and returns response
            return ParseReponseBody(responseText);
        }

        private string ParseReponseMultiPart(HttpWebResponse response)
        {
            if (response == null) { return null; }

            // get response text
            var responseContentType = response.ContentType;
            string responseText;
            using (var r = new StreamReader(response.GetResponseStream())) { responseText = r.ReadToEnd(); }

            // in case the response is not xml, throw an error with body text
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new ArgumentException(string.Format("HTTP {0}: {1}\r\n{2}",
                                            (int)response.StatusCode, response.StatusDescription, responseText));
            }
            else if (responseContentType.Contains("Multipart/Related"))
            {
                var content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(responseText)));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(responseContentType);

                // WARNING: make this recursive if required...
                var outerMultipart = content.ReadAsMultipartAsync().Result;
                if (outerMultipart.Contents.Count > 0)
                { responseText = outerMultipart.Contents[0].ReadAsStringAsync().Result; }
            }
            return responseText;
        }

        private string ParseReponseBody(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText)) { return null; }

            ResponseEnvelope = new SoapEnvelope();
            ResponseEnvelope.DocumentElement.OwnerDocument.PreserveWhitespace = true;

            try
            {
                ResponseEnvelope.LoadXml(responseText);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to parse [{ responseText }] to SOAP Response Message.");
            }

            CheckResponseSecurity();

            string responseBody = null;
            if (ResponseEnvelope.Body.HasChildNodes)
            {
                //responseBody = RemoveAllNamespaces(XElement.Parse(ResponseEnvelope.Body.InnerXml)).ToString();
                responseBody = ResponseEnvelope.Body.InnerXml;
            }

            return responseBody;
        }

        private void CheckResponseSecurity()
        {
            return;
            /* TODO: still outstanding response message security validation
            // How to: Create a Custom Policy Assertion that Secures SOAP Messages
            // https://msdn.microsoft.com/en-us/library/aa528788.aspx


            Implementing Message Layer Security with X.509 Certificates in WSE 3.0
            https://msdn.microsoft.com/en-us/library/ff648129.aspx
            */
        }

        #endregion Public


        #region Envelope Helpers
        private void EnvelopeAddHeader()
        {
            // create header
            var header = RequestEnvelope.CreateHeader();

            // complete header info
            this.NOnce = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            this.Created = DateTime.UtcNow;

            // add nodes
            var security = RequestEnvelope.CreateElement("Security", NS_WSSE);
            header.AppendChild(security);

            var token = RequestEnvelope.CreateElement("wsse", "UsernameToken", NS_WSSE);
            token.SetAttribute("xmlns:wsu", NS_WSU);
            token.SetAttribute("xmlns:wsse", NS_WSSE);
            security.AppendChild(token);

            EnvelopeAppendChild(token, "wsse", NS_WSSE, "Username", Username);
            EnvelopeAppendChild(token, "wsse", NS_WSSE, "Password", Password);
            EnvelopeAppendChild(token, "wsse", NS_WSSE, "Nonce", NOnce);
            EnvelopeAppendChild(token, "wsse", NS_WSSE, "Created", ConvertDateToXmlString(Created));
        }

        protected void EnvelopeAddBody(string actionName, string actionNamespace, Action<XmlElement, string> addParameters)
        {
            var body = RequestEnvelope.CreateBody();
            body.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            body.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

            var action = RequestEnvelope.CreateElement(actionName, actionNamespace);
            body.AppendChild(action);

            addParameters(action, actionNamespace);
        }

        protected XmlElement EnvelopeAppendChild(XmlElement parent, string prefix, string namespaceURI, string localName, string value = null)
        {
            var node = RequestEnvelope.CreateElement(prefix, localName, namespaceURI);
            if (!string.IsNullOrEmpty(value)) { node.InnerText = value; }
            parent.AppendChild(node);
            return node;
        }

        protected XmlElement EnvelopeAppendChild(XmlElement parent, string namespaceURI, string qualifiedName, string value = null)
        {
            var node = RequestEnvelope.CreateElement(qualifiedName, namespaceURI);
            if (!string.IsNullOrEmpty(value)) { node.InnerText = value; }
            parent.AppendChild(node);
            return node;
        }

        #endregion

        #region Static
        public static string ConvertIssueDateToXmlString(DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static string ConvertDateToXmlString(DateTime date)
        {
            return XmlConvert.ToString(date, XmlDateTimeSerializationMode.Local);
        }

        public static XElement RemoveAllNamespaces(XElement root)
        {
            // parse elements
            XElement node = new XElement(
                    root.Name.LocalName,
                    root.HasElements ?
                        root.Elements().Select(item => RemoveAllNamespaces(item)) :
                        (object)root.Value
                );
            // parse attributes
            node.ReplaceAttributes(root.Attributes().Where(attr => (!attr.IsNamespaceDeclaration)));
            // return
            return node;
        }

        public static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception) { return xml; }
        }
        #endregion
    }


}