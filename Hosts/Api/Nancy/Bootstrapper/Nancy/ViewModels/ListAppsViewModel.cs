using NotificationServer.Service.Entities;
using System.Collections.Generic;

namespace NotificationServer.Nancy.ViewModels
{
    public class ListAppsViewModel
    {
        public IEnumerable<Application> Applications { get; set; }

        public ListAppsViewModel()
        {
            Applications = new Application[] { };
        }
    }
}
