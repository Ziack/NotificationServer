using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Runtime.Services
{
    public interface ISleepService
    {
        void Sleep(int milliseconds);
    }
}
