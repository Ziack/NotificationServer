using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationServer.Contract;
using NotificationServer.Senders.Dian.Components.WebClients;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NotificationServer.Senders.Dian.Tests
{
    [TestClass()]
    public class DianSenderTests
    {
        public const string NIT_EMISOR = "900399741";
        public const string TIPO_DOC_FACTURA = "FACTURA-UBL";

        public string BaseDirectory { get { return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..")); } }
        public string App_Data { get { return Path.Combine(BaseDirectory, "App_Data"); } }

        public void T00_GetDocuments()
        {
            Get_Xml_ForDocument(7276);
        }

        private void Get_Xml_ForDocument(int repext_xmlDocumentos_id)
        {
            var dt = RepExt_XmlDocumentos_Get(repext_xmlDocumentos_id);
            Assert.IsNotNull(dt, "DataTable");
            Assert.AreEqual(1, dt.Rows.Count, "Row Count");

            var row = dt.Rows[0];

            var id = Convert.ToInt64(row["documento_id"]);
            var xml = Convert.ToString(row["documento_xml"]);
            var creado = Convert.ToDateTime(row["creado_dt"]);

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("", "");

            var issueDate = "";
        }

        private DataTable RepExt_XmlDocumentos_Get(int xmlDocumentos_id)
        {
            var repextCnnString = @"Server=FCDB1\DEV;Database=PLColabV2RepExt_Trunk_App;Integrated Security=True";
            var query = @"SELECT documento_id, documento_xml, creado_dt FROM XmlDocumentos WHERE espacio_de_nombres_contrato = @espacio_de_nombres_contrato";

            var dt = new DataTable("XmlDocumentos");
            using (var repextCnn = new SqlConnection(repextCnnString))
            {
                repextCnn.Open();

                var cmd = repextCnn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("espacio_de_nombres_contrato", xmlDocumentos_id);
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            }

            return dt;
        }

        public TestContext TestContext { get; set; }

        [TestMethod()]
        public void T01_Factura_08_18() { DianSendUBL("Factura_08-18-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_08_18_InputFmt() { DianSendUBL("Factura_08-18_InputFmt-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_08_24() { DianSendUBL("Factura_08-24-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_10_03() { DianSendUBL("Factura_10-03-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_10_06_Missing_Extension() { DianSendUBL("Factura_10-06-Missing_Extension-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_10_10_StepFirma() { DianSendUBL("Factura_10-10-StepFirma-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_10_10_Emision() { DianSendUBL("Factura_10-10-Emision-Signed.xml"); }

        [TestMethod()]
        public void T01_Factura_10_11_StepFirma() { DianSendUBL("Factura_10-11-StepFirma-Signed.xml"); }

        [TestMethod()]
        public void T01_SendFactura_900399741() { DianSendUBL("FACTURA-UBL_900399741.xml"); }

        [TestMethod()]
        public void T02_NotaCredito_08_22() { DianSendUBL("NotaCredito_08-22-Signed.xml"); }

        [TestMethod()]
        public void T02_NotaCredito_08_22_InputFmt_VS() { DianSendUBL("NotaCredito_08-22-Signed-InputFmt_VS.xml"); }

        [TestMethod()]
        public void T02_NotaCredito_08_22_InputFmt_Notepad() { DianSendUBL("NotaCredito_08-22-Signed-InputFmt_Notepad.xml"); }

        [TestMethod()]
        public void T03_NotaDebito_08_05() { DianSendUBL("NotaDebito_08-05-Signed.xml"); }




        [TestMethod()]
        public void T06_Send_ValidSignature_NotaCredito_990000004() { DianSendUBL(@"ValidSignature\face_c090039974100‭3B023384.xml"); }
        [TestMethod()]
        public void T06_Send_ValidSignature_NotaDebito_990000003() { DianSendUBL(@"ValidSignature\face_d090039974100‭3B023383.xml"); }
        [TestMethod()]
        //ERROR: El ejemplar no se pudo acceder
        public void T06_Send_ValidSignature_Factura_995900000() { DianSendUBL(@"ValidSignature\face_f09003647100000000001.xml"); }
        [TestMethod()]
        public void T06_Send_ValidSignature_Factura_990001001() { DianSendUBL(@"ValidSignature\face_f090039974100‭3B023769.xml"); }



        public void DianSendUBL(string embeddedFilename)
        {
            var options = new Dictionary<string, object>
            {
                {"Url", ConfigurationManager.AppSettings["Dian.Service.Url"]},
                {"Username", ConfigurationManager.AppSettings["Dian.Service.Username"]},
                { "Password", ConfigurationManager.AppSettings["Dian.Service.Password"]}
            };

            string xmlString = GetEmbedded(embeddedFilename);
            var docBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlString));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            var id = doc.DocumentElement.SelectSingleNode("//cbc:ID", ns).InnerText;
            var issueDate = doc.DocumentElement.SelectSingleNode("//cbc:IssueDate", ns).InnerText;
            var issueTime = doc.DocumentElement.SelectSingleNode("//cbc:IssueTime", ns).InnerText;
            var issueDateTime = string.Format("{0}T{1}", issueDate, issueTime);


            var notification = new Notification();
            notification.Properties.Add(new NotificationProperty { Key = "num_identificacion_emisor", Value = NIT_EMISOR });
            notification.Properties.Add(new NotificationProperty { Key = "numero_cd", Value = id });
            notification.Properties.Add(new NotificationProperty { Key = "cd_tipo_documento", Value = TIPO_DOC_FACTURA });
            notification.Properties.Add(new NotificationProperty { Key = "documento_xml_Base64", Value = docBase64 });
            notification.Properties.Add(new NotificationProperty { Key = "fecha_documento_dt", Value = issueDateTime });

            var sender = new DianSender(options);
            sender.HoldResponseEnvelope = true;

            sender.Send(notification);

            CheckResponse((string)sender.Response, sender.ResponseEnvelope);
        }


        private void CheckResponse(string response, string ResponseEnvelope)
        {
            Assert.IsNotNull(response, "response");
            TestContext.WriteLine("Response.Length: {0}", response.Length);
            TestContext.WriteLine("{0}", response);

            TestContext.WriteLine("");
            TestContext.WriteLine("FULL Response.Length: {0}", ResponseEnvelope.Length);
            TestContext.WriteLine("{0}", ResponseEnvelope);


            var xml = XDocument.Parse(response).Root;
            XNamespace ns = "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura";
            Assert.AreEqual("EnvioFacturaElectronicaRespuesta", xml.Name.LocalName);
            Assert.IsNotNull(xml.Element(ns + "Version"), "Version");
            //Assert.AreEqual("Componente DIAN", xml.Element(ns +"Version").Value);

            Assert.IsNotNull(xml.Element(ns + "ReceivedDateTime"), "ReceivedDateTime");
            Assert.IsNotNull(xml.Element(ns + "ResponseDateTime"), "ResponseDateTime");

            Assert.IsNotNull(xml.Element(ns + "Response"), "Response");
            Assert.IsNotNull(xml.Element(ns + "Comments"), "Comments");
            if (xml.Element(ns + "Response").Value != "200")
            { Assert.Fail(xml.Element(ns + "Comments").Value); }

            //Assert.AreEqual(expected:, response);
            /* Example:
            <EnvioFacturaElectronicaRespuesta>
              <Version>Componente DIAN</Version>
              <ReceivedDateTime>2016-09-01T13:23:36.488-05:00</ReceivedDateTime>
              <ResponseDateTime>2016-09-01T13:23:36.624-05:00</ResponseDateTime>
              <Response>320</Response>
              <Comments>Parámetros de solicitud de servicio web, no coincide contra el archivo. Parametros [ NIT=OK  DOCNUMBER=OK  ISSUEDATE=ERROR ]</Comments>
            </EnvioFacturaElectronicaRespuesta>

            <EnvioFacturaElectronicaRespuesta>
              <Version>Componente DIAN</Version>
              <ReceivedDateTime>2016-09-01T16:08:46.639-05:00</ReceivedDateTime>
              <ResponseDateTime>2016-09-01T16:08:46.986-05:00</ResponseDateTime>
              <Response>200</Response>
              <Comments>Ejemplar recibido exitosamente pasará a verificación.</Comments>
            </EnvioFacturaElectronicaRespuesta>
            */
        }



        #region Para Depurar sin DianSender

        [TestMethod]
        public void Send_Direct_Without_DianSender()
        {
            string url = ConfigurationManager.AppSettings["Dian.Service.Url"];
            string username = ConfigurationManager.AppSettings["Dian.Service.Username"];
            string password = ConfigurationManager.AppSettings["Dian.Service.Password"];

            var nit = "900399741";
            var invoiceNumber = "990002022";
            var issueDate = DateTime.Parse("2016-08-24T00:00:00");
            var document = GetSampleXml_Body_DocumentNode();

            var client = new DianFacturaElectronicaClient(url, username, password);
            var response = client.EnvioFacturaElectronica(nit, invoiceNumber, issueDate, document);

            CheckResponse((string)response, client.ResponseEnvelope.OuterXml);
        }

        private string GetSampleXml_Body_DocumentNode()
        {
            return @"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
AHobAAAAAA==";
        }
        #endregion



        #region Helpers

        public static string GetEmbedded(string fileNameInEmbeddedFolder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            fileNameInEmbeddedFolder = fileNameInEmbeddedFolder.Replace(@"\", ".");
            var resourceName = "NotificationServer.Senders.DianTests.Embedded." + fileNameInEmbeddedFolder;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) { throw new ArgumentNullException("resourceName", resourceName); }
                using (StreamReader reader = new StreamReader(stream)) { return reader.ReadToEnd(); }
            }
        }

        #endregion
    }
}