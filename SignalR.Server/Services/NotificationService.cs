using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Server.Helper;
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
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(IHubContext<NotificationHub, ITypedNotification> notificationHubContext,
            ILogger<NotificationService> logger)
        {
            _notificationHubContext = notificationHubContext;
            _logger = logger;
        }
        public Task SendNotification(string message)
        {
            _logger.Info($"Sending notification with message : {message}", _notificationHubContext);
            return Task.CompletedTask;
        }
    }
}
