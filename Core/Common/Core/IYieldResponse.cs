using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core
{
    public interface IYieldResponse
    {
        object Response { get; }

        String UrlSender { get; }
    }
}
