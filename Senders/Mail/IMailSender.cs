using NotificationServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NotificationServer.Contract;

namespace NotificationServer.Senders.Mail
{
    /// <summary>
    /// An object used to deliver mail.
    /// </summary>
    public interface IMailSender : ISender, IRequireRenderedBody, IDisposable
    {

    }
}
