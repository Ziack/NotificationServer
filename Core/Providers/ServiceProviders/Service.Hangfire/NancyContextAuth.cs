using Hangfire.Dashboard;
using Microsoft.Owin;
using Nancy.Security;
using System.Collections.Generic;
using System.Linq;

namespace NotificationServer.Service.Hangfire
{
    internal class NancyContextAuth : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owin)
        {

            var context = new OwinContext(owin);

            var authorized = false;

            if (owin.ContainsKey("Nancy.UserIdentity"))
            {
                var userIdentity = owin["Nancy.UserIdentity"] as IUserIdentity;
                authorized = userIdentity != null && userIdentity.Claims.Contains("ADMINISTRATOR");
            }

            if (!authorized)
                context.Response.Redirect("/Notifications/Dashboard/Login?returnUrl=" + HangfireNotificationsScheduler.HANGFIRE_PATH);

            return true;
        }
    }
}
