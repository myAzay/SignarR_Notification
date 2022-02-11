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
        private static void SendLog(IHubContext<LoggingHub, ITypedLogging> context, string message)
        {
            //TODO swith case of levels logs
            //notificationHubContext.Clients.All.ReceiveNotification(message);
            context.Clients.All.GetInfoLog(message);
        }
        private static void SendError(IHubContext<LoggingHub, ITypedLogging> context, string message)
        {
            context.Clients.All.GetErrorMessageLog(message);
        }
        private static string ValidateLogString(string message)
        {
            return $"{DateTime.Now} : {message}";
        }
        public static void Info(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogInformation(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
        
        public static void Error(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogError(logMessage);
            SendError(notificationHubContext, logMessage);
        }

        public static void Error(this ILogger logger, Exception error, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogError(error, logMessage);
            SendLog(notificationHubContext, logMessage);
        }

        public static void Warning(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogWarning(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
        
        public static void Debug(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogDebug(logMessage);
            SendLog(notificationHubContext, logMessage);
        }
    }
}
