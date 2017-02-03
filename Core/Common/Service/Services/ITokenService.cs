using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Services
{
    public interface ITokenService
    {
        string EncodeToken(object keyData);

        object DecodeToken(string token);
    }
}
