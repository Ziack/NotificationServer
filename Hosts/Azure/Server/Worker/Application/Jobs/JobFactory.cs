using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Transform.Application.Jobs
{
    public class JobFactory
    {
        public static IDictionary<string, IJob> Create()
        {
            var jobs = new Dictionary<string, IJob>();

            var executionPath = Environment.CurrentDirectory;
            var templatePath = Path.Combine(executionPath, "EmailTemplates");

            var emailDataProvider = new EmailDataProvider();
            jobs.Add("email", new MailJob(new Mailer(templatePath), emailDataProvider));

            return jobs;
        }
    }
}
