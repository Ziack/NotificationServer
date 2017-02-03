using NotificationServer.Contract;
using NotificationServer.Core;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Threading.Tasks;

namespace NotificationServer.Senders.Powershell
{
    public class PowershellSender : ISender, IRequireRenderedBody
    {
        public string RenderedBody { get; set; }

        public void Send(Notification mail)
        {
            using (var powerShellInstance = PowerShell.Create())
            {
                powerShellInstance.AddScript(script: RenderedBody);

                foreach (var item in mail.Properties)                
                    powerShellInstance.AddParameter(item.Key, item.Value);                

                Collection<PSObject> PSOutput = powerShellInstance.Invoke();

                if (powerShellInstance.Streams.Error.Count > 0)               
                    throw new Exception("One or more error has ocurred.");
            }            
        }

        public void SendAsync(Notification mail, Action<Notification> callback)
        {
            var task = Task.Run(() => Send(mail));
            task.ContinueWith(t => callback(mail));
        }
    }
}
