using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Entities
{
    public class User
    {
        public string Username { get; set; }

        public string EncryptedPassword { get; set; }

        public Guid Uid { get; set; }
    }
}
