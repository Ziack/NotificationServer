using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace NotificationServer.Core.Utilities
{
    public static class XmlHelper
    {
        public const string Ampersand = "&amp;";
        public const string Lsaquo = "&lt;";//single left-pointing angle quotation mark
        public const string Rsaquo = "&gt;";//single right-pointing angle quotation mark
        public const string Nbsp = "&#160;";
        public const string XSL_DEFINICION_ABRIR = @"<?xml version=""1.0"" encoding=""utf-8""?><xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""><xsl:output method=""html""/><xsl:template match=""/"">";
        public const string XSL_DEFINICION_CERRAR = @"</xsl:template></xsl:stylesheet>";

        /// <summary>
        /// Realiza la transformacion de un xml aplicandole un xslt
        /// </summary>
        /// <param name="xml">cadena xml a transformar</param>
        /// <param name="xsl">cadena xsl que se aplica al xml</param>
        /// <returns>string resultado de la transformacion</returns>
        public static string Transform(string xsl, string xml)
        {
            if (string.IsNullOrEmpty(xsl) || string.IsNullOrEmpty(xml))
            { return string.Empty; }

            XslCompiledTransform myXslTransform = new XslCompiledTransform();
            XPathDocument myXPathDocument;
            StringWriter stTransformacion = new StringWriter();
            //Xsl de transformacion
            XmlReader XSLDetalle = XmlReader.Create(new StringReader(xsl));
            //Xml del documento
            XmlReader XmlDoc = XmlReader.Create(new StringReader(xml));
            myXPathDocument = new XPathDocument(XmlDoc);
            myXslTransform.Load(XSLDetalle);
            myXslTransform.Transform(myXPathDocument, null, stTransformacion);

            return stTransformacion.ToString();
        }

        /// <summary>
        /// Realiza la transformacion de un xml aplicandole un xslt
        /// </summary>
        /// <param name="xml">cadena xml a transformar</param>
        /// <param name="xsl">cadena xsl que se aplica al xml</param>
        /// <param name="Arguments">parametros del xsl</param>
        /// <returns>string resultado de la transformacion</returns>
        public static string Transform(string xsl, string xml, Dictionary<string, string> Arguments)
        {
            //the outputs
            string result = string.Empty;

            try
            {
                //read XML
                TextReader txtReader4Xml = new StringReader(xml);
                XmlTextReader xmlReader4Xml = new XmlTextReader(txtReader4Xml);
                XPathDocument xPathDocument = new XPathDocument(xmlReader4Xml);

                //read XSLT
                TextReader txtReader4Xsl = new StringReader(xsl);
                XmlTextReader xmlReader4Xsl = new XmlTextReader(txtReader4Xsl);
                XslCompiledTransform xslt = new XslCompiledTransform();

                xslt.Load(xmlReader4Xsl);

                //create the output stream
                StringBuilder sb = new StringBuilder();
                TextWriter tw = new StringWriter(sb);

                XsltArgumentList args = new XsltArgumentList();
                if (Arguments != null)
                {
                    foreach (KeyValuePair<string, string> item in Arguments)
                    {
                        if (item.Value != null)
                        {
                            args.AddParam(item.Key, "", item.Value);
                        }
                    }
                }

                xslt.Transform(xPathDocument, args, tw);

                tw.Close();

                //get result
                result = sb.ToString();
            }
            // error handling
            catch (XsltException e) { throw DetailedException(e.LineNumber, e.LinePosition, e.InnerException, e.Message, xsl); }
            catch (XmlException e) { throw DetailedException(e.LineNumber, e.LinePosition, e.InnerException, e.Message, xsl); }
            catch (XPathException e) { throw DetailedException(0, 0, e.InnerException, e.Message, xsl); }

            return result;
        }

        /// <summary>
        /// Arma una XSL a partir de las XSL de la plantilla y de su plantilla master en caso de tenerla
        /// </summary>
        /// <param name="xslMaster">XSL de la plantilla master</param>
        /// <param name="xslPlantilla">XSL de la plantilla</param>
        /// <returns>XSL completo</returns>
        public static string BuildXSL(string xslMaster, string xslPlantilla, bool bAddDefinition = false)
        {
            string plantilla;

            // une la plantilla con la plantilla master en de que esta exista
            if (!string.IsNullOrEmpty(xslMaster))
            {
                plantilla = xslMaster.Replace("{CONTENT_TEMPLATE_BODY}", xslPlantilla);
            }
            else
            {
                plantilla = xslPlantilla;
            }

            // agrega una definicion basica para que la plantilla se pueda usar (util cuando se quiere visualizar sin aplicar la master)
            if (bAddDefinition)
            {
                plantilla = string.Format("{0}{1}{2}", XSL_DEFINICION_ABRIR, plantilla, XSL_DEFINICION_CERRAR);
            }

            return plantilla;
        }


        /// <summary>
        /// Helper del metodo Transform encargado de generar una exception de transformacion incluyendo la linea y posicion del error en el xsl
        /// </summary>
        /// <param name="LineNumber">Numero de la linea donde ocurrio el error</param>
        /// <param name="LinePosition">Posicion dentro de la linea donde ocurrio el error</param>
        /// <param name="InnerException">objeto para obtener el mensaje de error</param>
        /// <param name="Message">Mensaje de error</param>
        /// <param name="XslPath">Ruta del xsl</param>
        /// <returns></returns>
        private static Exception DetailedException(int LineNumber, int LinePosition, Exception InnerException, string Message, string XslPath)
        {
            return new Exception(string.Format("[Ln {0}, Col {1}] {2}\n{3}",
                                    LineNumber, LinePosition,
                                    InnerException != null ? InnerException.Message : Message,
                                    XslPath));
        }
    }
}