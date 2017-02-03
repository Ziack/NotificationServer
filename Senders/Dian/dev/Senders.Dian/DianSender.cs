using NotificationServer.Contract;
using NotificationServer.Core;
using NotificationServer.Senders.Dian.Components.WebClients;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NotificationServer.Senders.Dian
{
    public class DianSender : ISender, IYieldResponse
    {
        private string _Username;
        private string _Password;
        private string _Url;


        private string _xml;

        public string UrlSender { get { return _Url; } }
        public object Response { get { return _xml; } }

        public bool HoldResponseEnvelope { get; set; }
        public string ResponseEnvelope { get; private set; }


        public DianSender() { }

        public DianSender(IDictionary<string, object> options)
        {
            object output = null;
            if (options.TryGetValue("Url", out output)) { _Url = output.ToString(); }
            if (options.TryGetValue("Username", out output)) { _Username = output.ToString(); }
            if (options.TryGetValue("Password", out output)) { _Password = output.ToString(); }
        }

        public void Send(Notification notification)
        {
            var emisor = Convert.ToString(notification.Properties.FirstOrDefault(t => t.Key == "num_identificacion_emisor").Value);
            var receptor = Convert.ToString(notification.Properties.FirstOrDefault(t => t.Key == "num_identificacion_cd").Value);
            var uuid = Convert.ToString(notification.Properties.FirstOrDefault(t => t.Key == "uuid_cd").Value);
            var numero = Convert.ToString(notification.Properties.FirstOrDefault(t => t.Key == "numero_cd").Value);
            var tipoDocumento = Convert.ToString(notification.Properties.FirstOrDefault(t => t.Key == "cd_tipo_documento").Value);
            var documentoXmlBase64 = Convert.ToString(notification.Properties.FirstOrDefault(t => t.Key == "documento_xml_Base64").Value);
            var fechaDocumento = Convert.ToDateTime(notification.Properties.FirstOrDefault(t => t.Key == "fecha_documento_dt").Value);

            var zippedXmlFile = GetZippedAndBase64EncodedDocumentXml(emisor, numero, tipoDocumento, documentoXmlBase64);

            var client = new DianFacturaElectronicaClient(_Url, _Username, _Password);
            var rawResponse = client.EnvioFacturaElectronica(emisor, numero, fechaDocumento, zippedXmlFile);

            if (rawResponse == null) { _xml = null; return; }
            var response = XElement.Parse(rawResponse);

            var receivedInvoice = new XElement(name: (XName.Get("ReceivedInvoice", "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura")));
            receivedInvoice.SetElementValue(XName.Get("NumeroFactura", "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura"), numero);
            receivedInvoice.SetElementValue(XName.Get("Emisor", "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura"), emisor);
            receivedInvoice.SetElementValue(XName.Get("Receptor", "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura"), receptor);
            receivedInvoice.SetElementValue(XName.Get("UUID", "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura"), uuid);
            receivedInvoice.SetElementValue(XName.Get("Response", "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura"), String.Empty);            

            response.Add(receivedInvoice);
            if (HoldResponseEnvelope) { ResponseEnvelope = DianSoapClient.FormatXml(client.ResponseEnvelope.OuterXml); }

            _xml = Convert.ToString(response);
        }

        private string GetZippedAndBase64EncodedDocumentXml(string nit, string numero, string tipo, string xmlBase64)
        {
            var data = Convert.FromBase64String(xmlBase64);

            var fileName = _documentNameTransforms[tipo](nit, numero);

            var zippedFile = Zip(new Dictionary<string, byte[]> { { fileName, data } });

            return Convert.ToBase64String(zippedFile);
        }

        public static byte[] Zip(IDictionary<string, byte[]> files)
        {
            using (var compressedFileStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = zipArchive.CreateEntry(file.Key);

                        using (var originalFileStream = new MemoryStream(file.Value))
                        {
                            using (var zipEntryStream = zipEntry.Open())
                                originalFileStream.CopyTo(zipEntryStream);
                        }
                    }
                }

                return compressedFileStream.ToArray();
            }
        }

        public void SendAsync(Notification notification, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(notification));
            task.ContinueWith(t => callback(notification));
        }

        private static string CreateName(string typePrefix, string nit, string numero)
        {
            return string.Format("face_{0}{1}{2}.xml", "f", nit.PadLeft(10, '0'), numero.PadLeft(10, '0'));
        }

        private static IDictionary<string, Func<string, string, string>> _documentNameTransforms = new Dictionary<string, Func<string, string, string>>
        {
            { "FACTURA-UBL",     (nit, numero) => CreateName("f", nit, ToHex(numero))},
            { "NC-UBL",          (nit, numero) => CreateName("c", nit, ToHex(numero))},
            { "ND-UBL",          (nit, numero) => CreateName("d", nit, ToHex(numero))},
        };

        private static string ToHex(string number)
        {
            return Int64.Parse(number.Trim()).ToString("X");
        }

    }
}
