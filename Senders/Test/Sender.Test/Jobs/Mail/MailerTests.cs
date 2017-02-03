using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationServer.Provider.PuertoBahia.CachingProvider;
using NotificationServer.Provider.PuertoBahia.ServiceConfiguration;
using NotificationServer.Provider.PuertoBahia.TemplateResolvers;
using NotificationServer.Provider.PuertoBahia.TemplateServices;
using NotificationServer.Azure.Application.Jobs.Mail;
using NotificationServer.Senders.Mail.Senders;

namespace NotificationServer.Sender.Test.Jobs.Mail
{
    [TestClass]
    public class MailerTests
    {
        [TestMethod]
        public void MailerRendersEmail()
        {
            var mailer = new Mailer(
                templateService: new XSLTTemplateEngineService(
                    new PuertoBahiaTemplateServiceConfiguration(
                        resolver: new PuertoBahiaTemplateResolver(),
                        cachingProvider: new PuertoBahiaCachingProvider()
                        )
                    ),
                sender: new SmtpMailSender(new System.Net.Mail.SmtpClient { })
            );

            var properties = new Dictionary<string, object>();
            properties.Add("Remitente", "MARGO");
            properties.Add("Solicitud", "789256");
            properties.Add("Descripcion", "Solicitud Contrato prueba de email");
            properties.Add("Enlace", "http://dnndev.me/");

            var email = mailer.Run(
                templateName: "Solicitud_Contrato",
                from: "foo@acme.co",
                subject: "Foo Object",
                to: new[] { "jdoe@acme.co" },
                CC: new[] { "jdoe_cc@acme.co" },
                BCC: new[] { "jdoe_bcc@acme.co" },
                replyTo: new[] { "jdoe_reply@acme.co" },
                properties: properties,
                attachment: null
            );

            /*var body = new StreamReader(email.Notification.AlternateViews[0].ContentStream).ReadToEnd();

            Assert.IsNotNull(body);*/
        }
    }
}
