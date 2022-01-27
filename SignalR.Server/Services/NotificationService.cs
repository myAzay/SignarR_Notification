using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Hubs;
using SignalR.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, ITypedNotification> _notificationHubContext;
        public NotificationService(IHubContext<NotificationHub, ITypedNotification> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }
        public Task SendNotification(string message)
        {
            return _notificationHubContext.Clients.All.ReceiveNotification(message);
        }
    }
}
