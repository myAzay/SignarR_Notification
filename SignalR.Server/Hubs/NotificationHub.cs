using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Hubs
{
    public class NotificationHub : Hub<ITypedNotification>
    {
        public Task SendNotification(string message)
        {
            return Clients.All.ReceiveNotification(message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
