using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract.Utilities
{
    public static class IListExtensions
    {
        public static void AddAll<T>(this IList<T> self, IEnumerable<T> other)
        {
            if (self == null || other == null)
                return;

            foreach (var item in other)
                self.Add(item);
        }
    }
}
