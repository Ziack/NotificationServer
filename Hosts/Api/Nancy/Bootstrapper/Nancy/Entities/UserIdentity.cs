using Nancy.Security;
using System.Collections.Generic;

namespace NotificationServer.Nancy.Entities
{
    public class UserIdentity : IUserIdentity
    {
        private string _username;
        private IEnumerable<string> _claims = new string[] { };

        public UserIdentity(string name, IEnumerable<string> claims)
        {
            _username = name;
            _claims = claims;
        }

        public IEnumerable<string> Claims
        {
            get { return _claims; }
        }

        public string UserName
        {
            get { return _username; }
        }
    }
}
