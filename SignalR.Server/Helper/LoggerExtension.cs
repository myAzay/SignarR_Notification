using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalR.Server.Hubs;
using SignalR.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Helper
{
    public static class LoggerExtension
    {
        private static void SendLog(IHubContext<NotificationHub, ITypedNotification> notificationHubContext, string message)
        {
            notificationHubContext.Clients.All.ReceiveNotification(message);
        }
        private static string ValidateLogString(string message)
        {
            return $"{DateTime.Now} : {message}";
        }
        public static void Info(this ILogger logger, string message, IHubContext<NotificationHub, ITypedNotification> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogInformation(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
        
        public static void Error(this ILogger logger, string message, IHubContext<NotificationHub, ITypedNotification> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogError(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
        
        public static void Warning(this ILogger logger, string message, IHubContext<NotificationHub, ITypedNotification> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogWarning(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
        
        public static void Debug(this ILogger logger, string message, IHubContext<NotificationHub, ITypedNotification> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogDebug(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
    }
}
