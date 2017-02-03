using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities
{
    public class UserTableEntity : TableEntity
    {
        public String Username { get; set; }

        public String EncryptedPassword { get; set; }
    }
}
