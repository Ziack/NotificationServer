using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities.Extensions
{
    public static class UserTableEntityExtensions
    {
        public static User ToUser(this UserTableEntity entity)
        {
            return new User
            {
                Username = entity.Username,
                EncryptedPassword = entity.EncryptedPassword,
                Uid = Guid.Parse(entity.RowKey)
            };
        }
    }
}
