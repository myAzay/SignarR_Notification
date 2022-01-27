using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(string message);
    }
}
