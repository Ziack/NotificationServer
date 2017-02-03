using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace NotificationServer.Senders.SPRC.Tests
{
    [TestClass]
    public class SPRCSenderTests
    {
        [TestMethod]
        public void Send_CorrectCredential_DoesNotThrowAnyException()
        {
            var options = new Dictionary<String, Object>
            {
                { "Host", "http://delfin.puertocartagena.com" },
                { "Port", 8000 },
                { "Auth.Resource", "/gateway-in-plcolab/auth/login" },
                { "Auth.Method", "POST" },
                { "Update.Resource", "/gateway-in-plcolab/documents/update" },
                { "Update.Method", "PUT" },
                { "Headers", new Dictionary<String, String> { { "ContentType", "application/json" }, { "Accept", "application/json" } } },
                { "User", "plcolab" },
                { "Password", "$Plcolab&V1" }
            };
            var sender = MakeSender(options);

            sender.Send(mail: null);
        }

        private static SPRCSender MakeSender(Dictionary<String, Object> options)
        {
            return new SPRCSender(options);
        }

        [TestMethod]
        public void Send_DocumentInRenderBody_DoesNotThrowAnyException()
        {
            var options = new Dictionary<String, Object>
            {
                { "Host", "http://delfin.puertocartagena.com" },
                { "Port", 8000 },
                { "Auth.Resource", "/gateway-in-plcolab/auth/login" },
                { "Auth.Method", "POST" },
                { "Update.Resource", "/gateway-in-plcolab/documents/update" },
                { "Update.Method", "PUT" },
                { "Headers", new Dictionary<String, String> { { "ContentType", "application/json" }, { "Accept", "application/json" } } },
                { "User", "plcolab" },
                { "Password", "$Plcolab&V1" }
            };
            var renderBody = @"{
""date"": ""2016-12-06 14:32:34.471"",
""documentNumber"": ""990096320"",
""status"": ""200"",
""documentType"" : ""FACTURA-UBL""
}";
            var sender = MakeSender(options);
            sender.RenderedBody = renderBody;

            sender.Send(mail: null);
        }
    }
}
