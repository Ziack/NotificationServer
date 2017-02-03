using NotificationServer.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.TableEntities.Extensions
{
    public static class ApplicationTableEntityExtensions
    {
        public static Application ToApplication(this ApplicationTableEntity entity)
        {
            return new Application
            {
                 Description = entity.Description,
                 Name = entity.Name,
                 Token = entity.Token
            };
        }
    }
}
