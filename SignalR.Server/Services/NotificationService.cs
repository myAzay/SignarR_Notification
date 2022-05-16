using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Server.AppExceptions;
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
        private readonly ILoggerWrapper<NotificationService> _logger;
        public NotificationService(ILoggerWrapper<NotificationService> logger)
        {
            _logger = logger;
        }
        public Task SendNotification(string message)
        {
            switch (message)
            {
                case "1":
                    _logger.Warning("Warning message");
                    break;
                case "0":
                    _logger.Error("Error message");
                    break;
                case "-1":
                    throw new AppException(400, "Error message");
                default:
                    _logger.Info($"Sending notification with message : {message}");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
