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
    public class SqlServerUsersRepository : SqlServerRepositoryBase, IUsersRepository
    {
        public SqlServerUsersRepository()
            : base(null)
        {

        }

        public SqlServerUsersRepository(string connectionString)
            : base(connectionString)
        {

        }

        public void ChangePassword(string username, string encryptedPass)
        {
            Exec(conn => conn.Execute("Notificacion.pr_Usuarios_CambiarClave", new
            {
                nombre_cd = username,
                clave_cd = encryptedPass,
            }));
        }

        public bool Exists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");


            var result = Exec(conn => conn.Single<dynamic>("Notificacion.pr_Usuarios_Get", new
            {
                nombre_cd = username,
                clave_cd = (string)null,
                uid_cd = (Guid?)null
            }));

            return result != null;
        }

        public User Get(string username)
        {
            var result = Exec(conn => conn.Single<dynamic>("Notificacion.pr_Usuarios_Get", new
            {
                nombre_cd = username,
                clave_cd = (string)null,
                uid_cd = (string)null,
            }));

            if (result == null)
                return null;

            return new User
            {
                Username = result.nombre_cd,
                EncryptedPassword = result.clave_cd,
                Uid = result.uid_cd,
            };
        }

        public User Get(Guid uid)
        {
            var result = Exec(conn => conn.Single<dynamic>("Notificacion.pr_Usuarios_Get", new
            {
                nombre_cd = (string)null,
                clave_cd = (string)null,
                uid_cd = uid
            }));

            if (result == null)
                return null;

            return new User
            {
                Username = result.nombre_cd,
                EncryptedPassword = result.clave_cd,
                Uid = result.uid_cd,
            };
        }

        public User Get(string username, string encryptedPass)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");

            if (string.IsNullOrWhiteSpace(encryptedPass))
                throw new ArgumentNullException("encryptedPass");

            var result = Exec(conn => conn.Single<dynamic>("Notificacion.pr_Usuarios_Get", new
            {
                nombre_cd = username,
                clave_cd = encryptedPass,
                uid_cd = (Guid?)null
            }));

            if (result == null)
                return null;

            return new User
            {
                Username = result.nombre_cd,
                EncryptedPassword = result.clave_cd,
                Uid = result.uid_cd,
            };
        }

        public void Insert(User user)
        {
            Exec(conn => conn.Execute("Notificacion.pr_Usuarios_Insert", new
            {
                nombre_cd = user.Username,
                clave_cd = user.EncryptedPassword,
                uid_cd = user.Uid,
                id_UsuarioCreador = 1
            }));
        }
    }
}
