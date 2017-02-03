using Nancy;
using Nancy.Security;
using NotificationServer.Nancy.ViewModels;
using NotificationServer.Service.Repositories;
using NotificationServer.Service.Services;

namespace NotificationServer.Nancy.Modules
{
    public class AppsModule : NancyModule
    {
        private IApplicationsRepository _applications;
        private ITokenService _tokens;

        public AppsModule(IApplicationsRepository applications, ITokenService tokens)
            : base("/Notifications/Dashboard/Apps")
        {
            _applications = applications;
            _tokens = tokens;

            this.RequiresAuthentication();

            Get["/"] = Apps;

            Put["/{appName}/Token"] = SetAppToken;
        }

        private dynamic SetAppToken(dynamic arg)
        {
            var appName = arg.appName;
            var token = _tokens.EncodeToken(new { app = arg.appName });

            _applications.SetToken(appName, token);

            return Response.AsJson("Ok");
        }

        private dynamic Apps(dynamic arg)
        {
            var model = new ListAppsViewModel();

            model.Applications = _applications.List(0, 10);

            return Negotiate
                .WithModel(model)
                .WithView("Views/Apps/Index");
        }


    }
}