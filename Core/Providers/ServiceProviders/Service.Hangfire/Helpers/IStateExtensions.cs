using Hangfire;
using Hangfire.Batches.States;
using Hangfire.Common;
using Hangfire.States;
using NotificationServer.Service.Entities;
using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Hangfire.Helpers
{
    public static class IStateExtensions
    {
        public static void Run(this IState state, NotificationSpec spec, Expression<Action> exec)
        {
            var bjState = new EnqueuedState { Queue = $"{ spec.Priority }".ToLower() };

            IBackgroundJobClient client = new BackgroundJobClient();
            var jobId = client.Create(exec, bjState);

            var storageConnection = JobStorage.Current.GetConnection();
            storageConnection.SetJobParameter(id: jobId, name: "PartitionKey", value: JobHelper.ToJson(spec.PartitionKey));
            storageConnection.SetJobParameter(id: jobId, name: "MessageId", value: JobHelper.ToJson(spec.MessageId));
            storageConnection.SetJobParameter(id: jobId, name: "Priority", value: JobHelper.ToJson(spec.Priority.ToString()));
            storageConnection.SetJobParameter(id: jobId, name: "ExpiredAt", value: JobHelper.SerializeDateTime(DateTime.Now.AddHours(1)));
            
        }

        public static void Run(this IBatchState state, INotificationsRepository notificationRepository, NotificationSpec spec, Expression<Action> exec, String description = null)
        {
            var batchId = spec.Options.ContainsKey("BatchId") ? (Guid)spec.Options["BatchId"] : Guid.NewGuid();
            notificationRepository.AddToBatch(notificationId: spec.MessageId, batchId: batchId);

            //BatchJob.StartNew((x) =>
            //{
            //    x.Enqueue(exec);

            //}, description);
        }
    }
}