using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using NotificationServer.Service.Services;
using System;

namespace NotificationServer.Nancy.Entities
{
    public class UserMapper : IUserMapper
    {
        private UserManagementService _users;

        public UserMapper(UserManagementService users)
        {
            _users = users;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = _users.Check(identifier);

            if (user == null)
                return null;

            return new UserIdentity(user.Username, new string[] { "ADMINISTRATOR" });
        }
    }
}
