using Hangfire.Logging;
using Newtonsoft.Json;
using NotificationServer.Core;
using NotificationServer.Core.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotificationServer.Service.Entities
{
    public class NotificationSpec
    {
        public Guid MessageId { get; set; }

        public Guid PartitionKey { get; set; }

        public string ServiceName { get; set; }

        public SchedulingType Scheduling { get; set; }

        public SchedulingPriority Priority { get; set; }

        public Encoding Encoding { get; set; }

        public IDictionary<string, object> Options { get; set; }

        public string SenderType { get; set; }

        public IEnumerable<string> InterceptorTypes { get; set; }

        private readonly static ILog Logger;

        static NotificationSpec()
        {
            NotificationSpec.Logger = LogProvider.For<NotificationSpec>();
        }

        public NotificationSpec()
        {
            Options = new Dictionary<string, object>();
            InterceptorTypes = Enumerable.Empty<String>();
            Priority = SchedulingPriority.Default;
        }

        public ISender BuildSender()
        {
            return (ISender)Activator.CreateInstance(Type.GetType(SenderType, throwOnError: true), Options);
        }

        public IEnumerable<INotificationInterceptor> BuildNotificationInterceptors()
        {
            return InterceptorTypes
                .Select(t =>
                    (INotificationInterceptor)Activator.CreateInstance(Type.GetType(t, throwOnError: true))
                );
        }

        public static NotificationSpec FromOptions(IDictionary<string, object> options)
        {
            var spec = new NotificationSpec();
            spec.Options = options;

            var value = new object();

            if (options.TryGetValue("ServiceName", out value))
                spec.ServiceName = value.ToString();
            else
                spec.ServiceName = "Unknown!";

            if (options.TryGetValue("Scheduling", out value))
                spec.Scheduling = (SchedulingType)Enum.Parse(typeof(SchedulingType), value.ToString(), true);

            if (options.TryGetValue("Priority", out value))
                spec.Priority = (SchedulingPriority)Enum.Parse(typeof(SchedulingPriority), value.ToString(), true);

            LogExtensions.Error(NotificationSpec.Logger, $"From Options!: {JsonConvert.SerializeObject(options)}");

            LogExtensions.Error(NotificationSpec.Logger, $"From OptionsKeys!: {JsonConvert.SerializeObject(options.Select(t => t.Key.ToString()))}");

            LogExtensions.Error(NotificationSpec.Logger, $"From OptionsValues!: {JsonConvert.SerializeObject(options.Select(t => t.Value?.ToString()))}");

            if (options.TryGetValue("SenderType", out value))
                spec.SenderType = value.ToString();
            else
                throw new InvalidOperationException("SenderType is not configured. Set the SenderType setting to an assembly qualified class name of an NotificationServer.Core.ISender implementation.");

            if (options.TryGetValue("Encoding", out value))
                spec.Encoding = Encoding.GetEncoding(value.ToString());

            if (options.TryGetValue("InterceptorTypes", out value))
                spec.InterceptorTypes = value as IEnumerable<string>;
            else
                spec.InterceptorTypes = new string[] { };


            return spec;
        }
    }
}