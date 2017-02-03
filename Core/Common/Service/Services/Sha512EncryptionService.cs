using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Services
{
    public class Sha512EncryptionService : IEncryptionService
    {
        public string Encrypt(string original)
        {
            var hasher = SHA512.Create();

            var bytes = Encoding.Unicode.GetBytes(original);

            var hashedBytes = hasher.ComputeHash(bytes);

            var hashedString = String.Empty;

            foreach (byte b in hashedBytes)
                hashedString += String.Format("{0:x2}", b);


            return hashedString;
        }
        
        public bool VerifyPassword(string password, string goodHash)
        {
            return Encrypt(password) == goodHash;
        }
    }
}
