using NotificationServer.Service.Repositories;
using System;
using NotificationServer.Service.Entities;
using Hangfire;
using System.Linq.Expressions;
using Newtonsoft.Json;
using NotificationServer.Core;
using Hangfire.States;
using Owin;
using Hangfire.Dashboard;
using Hangfire.Common;
using Hangfire.Batches.States;
using NotificationServer.Service.Hangfire.Helpers;

namespace NotificationServer.Service.Hangfire
{
    public class HangfireNotificationsScheduler : INotificationsScheduler
    {
        private readonly JobStorage _jobStorage;

        private readonly INotificationsRepository _notificationRepository;

        internal const string HANGFIRE_PATH = "/hangfire";

        public HangfireNotificationsScheduler()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public HangfireNotificationsScheduler(JobStorage jobStorage, INotificationsRepository notificationRepository) : this()
        {
            _jobStorage = jobStorage;
            _notificationRepository = notificationRepository;
        }

        public void Startup(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseStorage(_jobStorage);
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });

            app.UseNancy(
                options => options.PerformPassThrough = context =>
                {
                    return context.Request.Path.StartsWith(HANGFIRE_PATH);
                }
            );
            app.UseHangfireDashboard(HANGFIRE_PATH, new DashboardOptions
            {
                AppPath = "/Notifications/Dashboard",
                AuthorizationFilters = new IAuthorizationFilter[] { new NancyContextAuth() }
            });

            app.UseHangfireServer();
            // Maintance Jobs
            //RecurringJob.AddOrUpdate("purge-expired-jobs", () => HangfireNotificationsScheduler.PurgeExpiredJobs(), Cron.Hourly, TimeZoneInfo.Utc);
        }

        [DisableConcurrentExecution(10 * 60)]
        public static void PurgeExpiredJobs()
        {
            var connection = JobStorage.Current.GetConnection();
        }

        public void Add(NotificationRunner spec)
        {
            Expression<Action> exec = () => this.InvokeRunner(spec.NotificationSpec, spec.Notifications, spec.TemplateService);            

            switch (spec.NotificationSpec.Scheduling)
            {
                case SchedulingType.Inmediate:

                    var eState = new EnqueuedState { Queue = $"{ spec.NotificationSpec.Priority }".ToLower() };
                    eState.Run(spec.NotificationSpec, exec);

                    break;
                case SchedulingType.Delayed:

                    if (!spec.NotificationSpec.Options.ContainsKey("NotificationsScheduler.SecondsToExecution"))
                        throw new InvalidOperationException("NotificationsScheduler.SecondsToExecution is not configured");

                    var time = double.Parse(spec.NotificationSpec.Options["NotificationsScheduler.SecondsToExecution"].ToString());
                    var sState = new ScheduledState(enqueueIn: TimeSpan.FromSeconds(time));

                    sState.Run(spec.NotificationSpec, exec);

                    break;
                case SchedulingType.Batch:
                    var bState = new BatchStartedState();
                    bState.Run(notificationRepository: _notificationRepository, spec: spec.NotificationSpec, exec: exec);

                    break;
                default:
                    break;
            }            
        }

        public void InvokeRunner(NotificationSpec spec, INotificationsRepository notifications, ITemplateEngineService templateEngine)
        {
            new NotificationRunner(spec, notifications, templateEngine).Run();
        }
    }
}
