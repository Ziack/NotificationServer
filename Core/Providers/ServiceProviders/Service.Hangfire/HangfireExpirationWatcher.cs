using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Hangfire.Pro.Redis;
using Hangfire.Storage;
using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;
using Hangfire.Logging;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;

namespace NotificationServer.Service.Hangfire
{
    internal class HangfireExpirationWatcher : IServerComponent
    {

        private readonly RedisStorageOptions _options;

        private readonly ConnectionMultiplexer _multiplexer;        

        private readonly IStorageConnection _connection;

        private readonly HangfireExpirationWatcherOption _watcherOption;

        private readonly static ILog Logger;

        static HangfireExpirationWatcher()
        {
            HangfireExpirationWatcher.Logger = LogProvider.For<HangfireExpirationWatcher>();
        }

        public HangfireExpirationWatcher(ConnectionMultiplexer multiplexer, RedisStorageOptions options, IStorageConnection connection, HangfireExpirationWatcherOption watcherOption)
        {
            _multiplexer = multiplexer;            
            _options = options;
            _connection = connection;
            _watcherOption = watcherOption;            
        }
        public void Execute(CancellationToken cancellationToken)
        {
            ConnectionMultiplexer connectionMultiplexer = this._multiplexer;
            int? databaseIndex = this._options.Database;
            var database = connectionMultiplexer.GetDatabase((databaseIndex.HasValue ? databaseIndex.GetValueOrDefault() : -1), null).WithKeyPrefix(this._options.Prefix);

            string[] stringArray = database.SetMembers("queues", CommandFlags.None).ToStringArray();

            Array.ForEach(stringArray, t => ProcessQueue(t, database));

            cancellationToken.WaitHandle.WaitOne(this._watcherOption.SleepTimeout);
        }

        private void ProcessQueue(string queue, IDatabase database)
        {
            LogExtensions.Debug(HangfireExpirationWatcher.Logger, $"Acquiring the lock for the fetched list of the '{ queue }' queue...");
            using (IDisposable disposable = _connection.AcquireDistributedLock($"queue:{ queue }:dequeued:lock", this._watcherOption.FetchedLockTimeout))
            {
                LogExtensions.Debug(HangfireExpirationWatcher.Logger, $"Looking for timed out jobs in the '{ queue }' queue...");
                int expiredJobs = 0;
                string[] stringArray = database.ListRange($"queue:{ queue }:dequeued", (long)0, (long)-1, CommandFlags.None).ToStringArray();

                Array.ForEach(stringArray, (jobId) =>
                {
                    if (this.IsExpired(database, jobId))
                    {
                        var stateChanger = new BackgroundJobStateChanger();
                        var changeContext = new StateChangeContext(
                            storage: JobStorage.Current, 
                            connection: _connection, 
                            backgroundJobId: jobId, 
                            newState: new FailedState(new Exception("Buajajajaja"))
                            );
                        stateChanger.ChangeState(changeContext);

                        expiredJobs++;
                    }
                });

                if (expiredJobs != 0)
                    LogExtensions.Info(HangfireExpirationWatcher.Logger, $"{ expiredJobs } timed out jobs were found in the '{ queue }' queue and re-queued.");
                else
                    LogExtensions.Debug(HangfireExpirationWatcher.Logger, $"No timed out jobs were found in the '{ queue }' queue");
            }
        }

        private bool IsExpired(IDatabase database, string jobId)
        {
            return true;

            var rawExpiredAt = _connection.GetJobParameter(jobId, "ExpiredAt");

            if (String.IsNullOrEmpty(rawExpiredAt))
                return false;

            var expiredAt = JobHelper.DeserializeDateTime(rawExpiredAt);

            return expiredAt < DateTime.Now;
        }
    }
}
