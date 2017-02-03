using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jose;
using System.Collections.Generic;

namespace GenerateTokensTest
{
    [TestClass]
    public class Tokentests
    {
        private static byte[] _secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

        struct Payload
        {
            public string app { get; set; }
        }


        [TestMethod]
        public void Encode()
        {
            var payload = new Payload
            {
                app = "TripleD"
            };

            string token = JWT.Encode(payload, _secretKey, JwsAlgorithm.HS256);

            var result = JWT.Decode<Payload>(token, _secretKey);

            Assert.AreEqual(payload.app, result.app);
        }

        [TestMethod]
        public void EncodeDynamic()
        {
            var payload = new Payload
            {
                app = "TripleD"
            };

            string token = JWT.Encode(payload, _secretKey, JwsAlgorithm.HS256);

            var result = JWT.Decode<dynamic>(token, _secretKey);
        }

        [TestMethod]
        public void DecodeDynamicFromObject()
        {
            var payload = new Payload
            {
                app = "TripleD"
            };

            string token = JWT.Encode(payload, _secretKey, JwsAlgorithm.HS256);

            var result = Decode(token) as IDictionary<string, object>;
            var app = result["app"];
        }

        private object Decode(string token)
        {
            return JWT.Decode<dynamic>(token, _secretKey);
        }
    }
}
