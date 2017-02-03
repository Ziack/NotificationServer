using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace NotificationServer.Core.Utilities
{
    public static class XmlValidationHelper
    {
        public static bool IsValid(string XML, string XSD, out string strMensajeError)
        {
            strMensajeError = string.Empty;
            try
            {
                // build XSD schema

                StringReader _XsdStream;
                _XsdStream = new StringReader(XSD);

                XmlSchema _XmlSchema;
                _XmlSchema = XmlSchema.Read(_XsdStream, null);

                // build settings (this replaces XmlValidatingReader)
                XmlReaderSettings _XmlReaderSettings;
                _XmlReaderSettings = new XmlReaderSettings()
                {
                    ValidationType = ValidationType.Schema
                };
                _XmlReaderSettings.Schemas.Add(_XmlSchema);

                // build XML reader
                StringReader _XmlStream;
                _XmlStream = new StringReader(XML);

                XmlReader _XmlReader;
                _XmlReader = XmlReader.Create(_XmlStream, _XmlReaderSettings);

                // validate
                using (_XmlReader)
                {
                    while (_XmlReader.Read()) ;
                }

                // validation succeeded
                return true;
            }
            catch (Exception ex)
            {
                // validation failed
                strMensajeError = ex.Message;
                return false;
            }
        }

    }
}
