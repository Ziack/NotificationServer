using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationServer.Client.Configuration;
using NotificationServer.Contract.Commands;
using System.Collections.Generic;
using NotificationServer.Contract;
using NotificationServer.Client.Providers.Rest.Exceptions;

namespace NotificationServer.Client.Rest.Tests
{
    [TestClass]
    public class ClientSenderTestsForAmazonSQS
    {
        [TestMethod]
        public void SendMail()
        {

            var client = NotificationClient.Providers["Amazon.Mail.SMTP"] as NotificationProvider;

            var result = client.Send(new NotifyCommand("Plcolab")
                .SentFrom("")
                .WithSubject("Testing Amazon SQS!")
                .WithDestination("Plcolab.Mail.Amazon", d => d
                    .AddTo("success@simulator.amazonses.com")
                    .AddTo("bounce@simulator.amazonses.com")
                    .AddTo("ooto@simulator.amazonses.com")
                    .AddTo("complaint@simulator.amazonses.com")
                    .AddTo("suppressionlist@simulator.amazonses.com")
                    .WithTemplate("TripleD/Mail/Prueba")
                )
                .WithProperty("Hello", "World")
                .WithProperty("Some Property", ":D")
                .WithProperty("Wiiiii", "Built with fluent interface!")
            );
        }

        [TestMethod]
        public void SendMailWithTag()
        {
            var client = NotificationClient.Provider;

            var result = client.Send(new NotifyCommand("TripleD")
                .SentFrom("notificator@fakedomain.com")
                .WithSubject("Test from NotificationServer.Client.Rest.Tests")
                .WithDestination("Mail", d => d
                    .AddTo("testing@fakedomain.com")
                    .WithTemplate("TripleD/Mail/Prueba")
                )
                .WithProperty("Hello", "World")
                .WithTag("Tag 1")
                .WithTag("Tag 2")
            );
        }


        [TestMethod]
        [ExpectedException(typeof(ErrorDePeticionException))]
        public void NotifyUnauthorized()
        {
            var client = NotificationClient.Providers["Rest.WrongCredentials"] as NotificationProvider;

            var result = client.Send(new NotifyCommand("TripleD")
                .SentFrom("jguevara@fakedomain.com")
                .WithSubject("Test from NotificationServer.Client.Rest.Tests")
                .WithDestination("Mail", d => d
                    .AddTo("testing@fakedomain.com")
                    .WithTemplate("TripleD/Mail/Prueba")
                )
                .WithProperty("Hello", "World")
                .WithProperty("Some Property", ":D")
                .WithProperty("Wiiiii", "Built with fluent interface!")
            );
        }

        [TestMethod]
        public void SendMailComplexObject()
        {
            var client = NotificationClient.Provider;


            var result = client.Send(new NotifyCommand("TripleD")
                .SentFrom("jguevara@fakedomain.com")
                .WithSubject("Test from NotificationServer.Client.Rest.Tests")
                .WithDestination("Mail", d => d
                    .AddTo("testing@fakedomain.com")
                    .WithTemplate("TripleD/Mail/PruebaObjetosComplejos")
                )
                .WithProperty("complex_property", new MyComplexObject
                {
                    MyProperty = "XD",
                    AnotherProperty = "XD 2"
                })
            );
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var client = NotificationClient.Provider;

            /* {
                 "NotificacionPOL": {
                     "Status": "@Prop("Status")",
     "Token": "@Prop("Token")",
     "Trasabilidad":  "@Prop("Trasabilidad")",
     "Valor_Pago":   "@Prop("Valor_Pago")",
     "Fecha_Pago":   "@Prop("Fecha_Pago")",
     "Banco_Cliente":   "@Prop("Banco_Cliente")",
     "Banco_Receptor":   "@Prop("Banco_Receptor")",
     "Canal_Pago":   "@Prop("Canal_Pago")"
                 },
   "cd_NumeroDocumento": "@Prop("cd_NumeroDocumento")",
   "cd_TipoDocumento": "@Prop("cd_TipoDocumento")",		
   "UserName":"@Prop("UserName")",
   "num_identificacion":"@Prop("num_identificacion")",
   "cd_TipoIdentificacion":"@Prop("cd_TipoIdentificacion")"
 }*/
            var result = client.Send(new NotifyCommand("TripleD")
                .SentFrom("jguevara@fakedomain.com")
                .WithSubject("Test from NotificationServer.Client.Rest.Tests")
                .WithDestination("Rest.TripleD.Recibir.Pago", d => d
                    .WithTemplate("TripleD/SendPago")
                )
                .WithProperty("complex_property", new MyComplexObject
                {
                    MyProperty = "XD",
                    AnotherProperty = "XD 2"
                })// Preguntarle a rafa sobre lo que manda del pago.
            );
        }

        struct MyComplexObject
        {
            public string MyProperty { get; set; }

            public string AnotherProperty { get; set; }
        }
    }
}