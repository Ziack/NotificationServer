using Nancy;
using Nancy.Security;

namespace NotificationServer.Nancy
{
    public class DashboardModule : NancyModule
    {
        public DashboardModule()
            : base("/Notifications/Dashboard")

        {
            this.RequiresAuthentication();

            Get["/"] = Index;
        }


        dynamic Index(dynamic _)
        {
            return View["Views/Dashboard/Index"];
        }
    }
}
