using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.Repositories
{
    public interface IApplicationsRepository
    {
        void SetToken(string applicationName, string token);

        IEnumerable<Application> List(int startPage, int pageSize);
    }
}
