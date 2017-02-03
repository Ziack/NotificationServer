using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Service.Entities;
using Insight.Database;

namespace NotificationServer.Service.SqlServer
{
    public class SqlServerApplicationsRepository : SqlServerRepositoryBase, IApplicationsRepository
    {
        public SqlServerApplicationsRepository()
            : base(null)
        {

        }

        public SqlServerApplicationsRepository(string connectionString)
            : base(connectionString)
        {

        }

        public IEnumerable<Application> List(int startPage, int pageSize)
        {
            return Exec(conn => conn.Query<dynamic>("Notificacion.pr_Aplicacion_Select").Select(a => new Application
            {
                Name = a.ds_Titulo,
                Description = a.ds_Descripcion,
                Token = a.ds_Token
            }));
        }

        public void SetToken(string applicationName, string token)
        {
            Exec(conn => conn.Execute("Notificacion.pr_Aplicacion_SetToken", new
            {
                ds_Titulo = applicationName,
                ds_Token = token,
            }));
        }
    }
}
