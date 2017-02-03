using log4net;
using log4net.Config;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Session;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using NotificationServer.Nancy.Lib;

namespace NotificationServer.Nancy.Init
{
    public class NotificationServerBootstrapperInternal : DefaultNancyBootstrapper
    {
        private ILog _log;

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(EmbeddedStaticContentConventionBuilder.AddDirectory("Views", GetType().Assembly));
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            CookieBasedSessions.Enable(pipelines);
            FormsAuthentication.Enable(pipelines, new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "~/Notifications/Dashboard/Login",
                UserMapper = container.Resolve<IUserMapper>(),
            });

            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(typeof(NotificationServerBootstrapperInternal));

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                var app = "";
                if (ctx.CurrentUser != null)
                    app = ctx.CurrentUser.UserName;

                _log.Info(NotificationRequestEventFactory.Crear(ctx.Request, ctx.Response, applicationName: app));
            });

            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                var app = "";
                if (ctx.CurrentUser != null)
                    app = ctx.CurrentUser.UserName;

                _log.Error(NotificationRequestEventFactory.Crear(ctx.Request, ctx.Response, ex, app));

                return null;
            });
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            ResourceViewLocationProvider
                .RootNamespaces
                .Add(GetType().Assembly, "NotificationServer.Nancy");
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
            }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }

        /*protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get
            {
                return new DiagnosticsConfiguration
                {
                    
                    Password = "abc123$"
                };
            }
        }*/
    }
}
