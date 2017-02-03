using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Contract
{
    [Serializable]
    public class NotificationProperty
    {
        public String Key { get; set; }
        public Object Value { get; set; }

        public NotificationProperty() { }

        public NotificationProperty(String key, Object value)
            : this()
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
