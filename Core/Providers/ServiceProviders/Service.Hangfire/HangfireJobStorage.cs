using Hangfire.Pro.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Server;
using Hangfire.Annotations;
using StackExchange.Redis;

namespace NotificationServer.Service.Hangfire
{
    public class HangfireJobStorage : RedisStorage
    {
        private const int DefaultSyncTimeout = 30000;

        private readonly ConfigurationOptions _configurationOptions;

        private readonly RedisStorageOptions _options;

        private readonly ConnectionMultiplexer _multiplexer;
        

        public HangfireJobStorage() : base() { }

        public HangfireJobStorage([NotNull] string configuration) : this(configuration, new RedisStorageOptions())
        {


        }

        public HangfireJobStorage([NotNull] string configuration, [NotNull] RedisStorageOptions options): base(configuration: configuration, options: options)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            this._configurationOptions = ConfigurationOptions.Parse(configuration);
            if (!options.AllowMultipleEndPointsWithoutRedLock && this._configurationOptions.EndPoints.Count > 1)
            {
                throw new ArgumentException("Multiple Redis endpoints aren't supported by Hangfire.Pro.Redis yet.", "configuration");
            }
            this._configurationOptions.ClientName = string.Format("Hangfire@{0}", Environment.GetEnvironmentVariable("COMPUTERNAME"));
            if (configuration.IndexOf("syncTimeout=", StringComparison.OrdinalIgnoreCase) < 0)
            {
                this._configurationOptions.SyncTimeout = 30000;
            }
            if (configuration.IndexOf("allowAdmin=", StringComparison.OrdinalIgnoreCase) < 0)
            {
                this._configurationOptions.AllowAdmin = true;
            }
            this._options = options;
            this._multiplexer = ConnectionMultiplexer.Connect(this._configurationOptions, null);
            this._multiplexer.PreserveAsyncOrder = false;
        }

        public override IEnumerable<IServerComponent> GetComponents()
        {
            var baseComponents = base.GetComponents().ToList();
            baseComponents.Add(new HangfireExpirationWatcher(
                multiplexer: _multiplexer, 
                options: _options, 
                connection: this.GetConnection(), 
                watcherOption: new HangfireExpirationWatcherOption {
                    SleepTimeout = TimeSpan.FromSeconds(60),
                    FetchedLockTimeout = TimeSpan.FromSeconds(60)
                }));

            return baseComponents;
        }
    }
}
