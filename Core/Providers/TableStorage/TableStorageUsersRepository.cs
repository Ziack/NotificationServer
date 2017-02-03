using NotificationServer.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Service.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using NotificationServer.Service.TableStorage.Helpers;
using NotificationServer.Service.TableStorage.TableEntities;
using NotificationServer.Service.TableStorage.TableEntities.Extensions;

namespace NotificationServer.Service.TableStorage
{
    public class TableStorageUsersRepository : TableStorageRepositoryBase, IUsersRepository
    {
        public override CloudTable Table { get; set; }

        public TableStorageUsersRepository(string connectionString) : base(connectionString: connectionString)
        {
            this.Table = CloudTableClient.GetTableReference("Users");
            this.Table.CreateIfNotExists();
        }

        public void ChangePassword(string username, string encryptedPass)
        {
            var query = Table.CreateQuery<UserTableEntity>()
                            .Where( t => t.Username == username && t.EncryptedPassword == encryptedPass);

            if (!query.ToList().Any())
                throw new ArgumentException($"User with username { username } does not exist.");

            var userToChange = query.Single();
            userToChange.EncryptedPassword = encryptedPass;

            TableOperation insertOperation = TableOperation.Replace(userToChange);
            Table.Execute(insertOperation);
        }

        public bool Exists(string username)
        {
            var query = Table.CreateQuery<UserTableEntity>()
                            .Where(t => t.Username == username);

            return query.ToList().Any();
        }

        public User Get(Guid uid)
        {
            var query = Table.CreateQuery<UserTableEntity>()
                            .Where(t => t.RowKey == Convert.ToString(uid));

            if (!query.ToList().Any())
                throw new ArgumentException($"User with key { uid } does not exist.");

            return query.Single().ToUser();            
        }

        public User Get(string username, string encryptedPass)
        {
            var query = Table.CreateQuery<UserTableEntity>()
                            .Where(t => t.Username == username && t.EncryptedPassword == encryptedPass);


            if (!query.ToList().Any())
                return null;

            return query.Single().ToUser();
        }

        public void Insert(User user)
        {
            TableOperation insertOperation = TableOperation.Insert(user.ToUserTableEntity());
            Table.Execute(insertOperation);
        }

        public User Get(string username)
        {
            var query = Table.CreateQuery<UserTableEntity>().Where(t => t.Username == username);


            if (!query.ToList().Any())
                return null;

            return query.Single().ToUser();
        }
    }
}
