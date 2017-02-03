using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Repositories
{
    public interface IUsersRepository
    {
        bool Exists(String username);

        User Get(String username);

        User Get(Guid uid);

        User Get(String username, String encryptedPass);        

        void Insert(User user);

        void ChangePassword(String username, String encryptedPass);
    }
}
