using System;

namespace NotificationServer.Senders.Dian.Components.WebClients
{
    public class DianFacturaElectronicaClient : DianSoapClient
    {
        public DianFacturaElectronicaClient(string url, string username, string password) : base(url, username, password) { }

        public string EnvioFacturaElectronica(string nit, string invoiceNumber, DateTime issueDate, string document)
        {
            return Send(actionName: "EnvioFacturaElectronicaPeticion",
                        actionNamespace: "http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura",
                        addParameters: (action, ns) => {
                            EnvelopeAppendChild(action, ns, "NIT", nit);
                            EnvelopeAppendChild(action, ns, "InvoiceNumber", invoiceNumber);
                            EnvelopeAppendChild(action, ns, "IssueDate", ConvertIssueDateToXmlString(issueDate));
                            EnvelopeAppendChild(action, ns, "Document", document);
                        });
        }
    }
}