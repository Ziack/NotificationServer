using NotificationServer.Service.Commands;
using NotificationServer.Service.Entities;
using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Services
{
    public class UserManagementService
    {
        private IUsersRepository _users;

        private IEncryptionService _encrypter;
        
        public UserManagementService(IUsersRepository users, IEncryptionService encrypter)
        {
            _users = users;
            _encrypter = encrypter;        
        }


        public void CreateUser(CreateUserCommand user)
        {
            if (_users.Exists(user.Username))
                throw new ArgumentException(string.Format("El usuario {0} ya existe", user.Username), "user");

            var newUser = new User
            {
                Username = user.Username,
                EncryptedPassword = _encrypter.Encrypt(user.UnencryptedPassword),
                Uid = Guid.NewGuid()
            };

            _users.Insert(newUser);
        }

        public User Login(string username, string unencryptedPassword)
        {            
            var user = _users.Get(username);
            if (user == null)
                return null;

            if (!_encrypter.VerifyPassword(unencryptedPassword, user.EncryptedPassword))
                return null;

            return user;
        }

        public User Check(Guid id)
        {
            return _users.Get(id);
        }

        public void ChangePassword(string username, string unencryptedPass)
        {
            var encryptedPassword = _encrypter.Encrypt(unencryptedPass);

            _users.ChangePassword(username, encryptedPassword);
        }

    }
}
