using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Interfaces
{
    public interface ITypedNotification
    {
        Task ReceiveNotification(string message);
    }
}
