using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Providers
{
    public interface IAllowConfigurationOverride
    {
        ITemplateServiceConfiguration Config { get; set; }
    }
}
