using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Service.TableStorage.Helpers
{
    public static class Prop
    {
        public static String Of<T>(Func<T, Object> exp)
        {
            //MemberExpression body = exp.Method.GetParameters();

            //if (body == null)
            //{
            //    UnaryExpression ubody = (UnaryExpression)exp.Body;
            //    body = ubody.Operand as MemberExpression;
            //}

            //return body.Member.Name;

            return String.Empty;
        }
    }
}
