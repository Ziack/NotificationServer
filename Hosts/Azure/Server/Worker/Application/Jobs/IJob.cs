using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Transform.Application.Jobs
{
    public interface IJob
    {
        void ParseJobMessage(string[] messageParts);

        void Execute();
    }
}
