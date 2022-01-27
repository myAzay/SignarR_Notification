﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public Task SendMessage(string message)
        {
            return Clients.All.SendAsync("ReceiveMessage",message);
        }
    }
}
