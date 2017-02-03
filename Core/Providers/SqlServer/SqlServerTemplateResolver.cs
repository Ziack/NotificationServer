using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;

namespace NotificationServer.Service.SqlServer
{
    public class SqlServerTemplateResolver : SqlServerRepositoryBase, ITemplateResolver
    {
        public SqlServerTemplateResolver()
            : base(null)
        {

        }

        public SqlServerTemplateResolver(string connectionString)
            : base(connectionString)
        {

        }

        public string Resolve(string name, IDictionary<string, object> parameters = null)
        {

            var template = (string)null;

            try
            {
                template = Exec(con => con.ExecuteScalar<string>("Notificacion.pr_Plantillas_Get", new { nm_nombre = name }));
            }
            catch (Exception ex)
            {
                throw new TemplateResolvingException("Could not load template, see inner exception for details.", ex);
            }

            if (template == null)
                throw new TemplateResolvingException();

            return template;
        }
    }
}
