using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities.Extensions
{
    public static class UserExtensions
    {
        public static UserTableEntity ToUserTableEntity(this User entity)
        {
            return new UserTableEntity
            {
                Username = entity.Username,
                EncryptedPassword = entity.EncryptedPassword,
                RowKey = Convert.ToString(entity.Uid)
            };
        }
    }
}
