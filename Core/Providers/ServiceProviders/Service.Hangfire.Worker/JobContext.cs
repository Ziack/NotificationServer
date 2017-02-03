using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Storage;

namespace Service.Hangfire.Worker
{
    class JobContext : IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        public void OnCreated(CreatedContext filterContext)
        {
            
        }

        public void OnCreating(CreatingContext filterContext)
        {
            
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            //var notificationSpec = filterContext.GetJobParameter<NotificationSpec>("notificationSpec");
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            
        }

        public void OnStateElection(ElectStateContext context)
        {
            
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            
        }
    }
}
