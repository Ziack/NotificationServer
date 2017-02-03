using Hangfire;
using Jose;
using log4net;
using log4net.Config;
//using Jose;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Newtonsoft.Json;
using NotificationServer.Config;
using NotificationServer.Nancy.Entities;
using NotificationServer.Service;
using NotificationServer.Service.JWT;
using NotificationServer.Service.Repositories;
using NotificationServer.Service.Services;
using NotificationServer.Service.SqlServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NotificationServer.Nancy.DefaultImplementation
{
    public class ConfigureNotificationServer : IRegistrations, IApplicationStartup
    {
        private static ILog Log { get { return LogManager.GetLogger(typeof(ConfigureNotificationServer)); } }

        private static readonly ConcurrentBag<BackgroundJobServer> Servers = new ConcurrentBag<BackgroundJobServer>();

        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get
            {
                return new CollectionTypeRegistration[] { };
            }
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get
            {
                var notificationServerConfiguration = (ConfigurationManager.GetSection("notificationService") as NotificationServiceConfigSection);
                var notificationService = notificationServerConfiguration.Build();

                var usersReporitory = notificationServerConfiguration.Users.Build<IUsersRepository>();
                var applicationReporitory = notificationServerConfiguration.Applications.Build<IApplicationsRepository>();
                var encryptionService = new PBKDF2EncryptionService();
                var userManagerService = new UserManagementService(usersReporitory, encryptionService);
                var userMapper = new UserMapper(userManagerService);

                return new InstanceRegistration[] {
                    Register<IApplicationsRepository>(applicationReporitory),
                    Register<NotificationServerService>(notificationService),
                    Register<IUsersRepository>(usersReporitory),
                    Register<IEncryptionService>(encryptionService),
                    Register<UserManagementService>(userManagerService),
                    Register<IUserMapper>(userMapper),
                    Register<ITokenService>(GetTokenService()),
                };
            }
        }

        public IEnumerable<TypeRegistration> TypeRegistrations
        {
            get
            {
                return new TypeRegistration[] { };
            }
        }

        public void Initialize(IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;
            XmlConfigurator.Configure();

            pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx =>
            {
                //try
                //{
                if (ctx.Request.Headers.Keys.Contains("X-Token"))
                {
                    var token = ctx.Request.Headers["X-Token"].FirstOrDefault();

                    Log.Error($"TOKEN: {token}");

                    var data = GetTokenService().DecodeToken(token) as IDictionary<string, object>;

                    Log.Error($"TOKENDATA: {JsonConvert.SerializeObject(data)}");

                    if (!data.ContainsKey("app"))
                        throw new Exception("Authorization error");

                    var appName = (data["app"] as IDictionary<string, object>)["Value"] as string;

                    Log.Error($"APPNAME:{appName}");

                    ctx.CurrentUser = new UserIdentity(appName, new string[] { "APP" });
                }

                var owin = ctx.GetOwinEnvironment();

                owin["Nancy.UserIdentity"] = ctx.CurrentUser;

                Log.Error("GETOWINENVIRONMENT");

                //}
                //catch (Exception ex)
                //{
                //    Log.Error("Error de autorización", ex);
                //    throw new Exception("Error deserializing token", ex);
                //}


                return null;
            });

            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                Log.Error(ctx.Request, ex);

                return null;
            });
        }

        private InstanceRegistration Register<TInstance>(object implementation)
        {
            if (!(implementation is TInstance))
                throw new ArgumentException(string.Format("implementation must be an instance of {0}", typeof(TInstance).Name), "implementation");

            return new InstanceRegistration(typeof(TInstance), implementation);
        }

        private ITokenService GetTokenService()
        {
            var config = ConfigurationManager.AppSettings["Tokens.Secret"];
            var alg = ConfigurationManager.AppSettings["Tokens.Algorithm"] ?? "HS256";

            Log.Error($"SECRET: {config} - ALGORITHM: {alg}");

            var bytes = config.Split(' ').Select(byte.Parse).ToArray();
            var algorithm = (JwsAlgorithm)Enum.Parse(typeof(JwsAlgorithm), alg);
            return new JwtTokenService(bytes, algorithm);
        }
    }

}
