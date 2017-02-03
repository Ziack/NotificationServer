using Jose;
using NotificationServer.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.JWT
{
    public class JwtTokenService : ITokenService
    {
        private JwsAlgorithm _algorithm;
        private byte[] _secret;

        public JwtTokenService(byte[] secret, JwsAlgorithm algorithm)
        {
            _secret = secret;
            _algorithm = algorithm;
        }

        public object DecodeToken(string token)
        {
            return Jose.JWT.Decode<dynamic>(token, _secret);
        }

        public string EncodeToken(object keyData)
        {
            return Jose.JWT.Encode(keyData, _secret, _algorithm);
        }
    }
}
