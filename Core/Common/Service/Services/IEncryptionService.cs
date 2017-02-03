using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Services
{
    public interface IEncryptionService
    {
        String Encrypt(String original);

        Boolean VerifyPassword(String password, String goodHash);
    }
}
