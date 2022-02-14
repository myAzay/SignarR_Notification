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
        private static void SendLog(IHubContext<LoggingHub, ITypedLogging> context, string message, LogLevel logLevel, Exception exception = null)
        {
            switch (logLevel)
            {
                case LogLevel.Warning:
                    context.Clients.All.GetWarningLog(message);
                    break;
                case LogLevel.Error:
                    if (exception is null)
                        context.Clients.All.GetErrorMessageLog(message);
                    else
                        context.Clients.All.GetErrorLog(exception.StackTrace, message);
                    break;
                case LogLevel.Debug:
                    context.Clients.All.GetDebugLog(message);
                    break;
                default:
                    context.Clients.All.GetInfoLog(message);
                    break;                 
            }
        }
        private static string ValidateLogString(string message)
        {
            return $"{DateTime.Now} : {message}";
        }
        public static void Info(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogInformation(logMessage);
            SendLog(notificationHubContext, logMessage, LogLevel.Info);
        }
        
        public static void Error(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogError(logMessage);
            SendLog(notificationHubContext, logMessage, LogLevel.Error);
        }

        public static void Error(this ILogger logger, Exception error, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogError(error, logMessage);
            SendLog(notificationHubContext, logMessage, LogLevel.Error, error);
        }

        public static void Warning(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogWarning(logMessage);
            SendLog(notificationHubContext, logMessage, LogLevel.Warning);
        }
        
        public static void Debug(this ILogger logger, string message, IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            var logMessage = ValidateLogString(message);
            logger.LogDebug(logMessage);
            SendLog(notificationHubContext, logMessage, LogLevel.Debug);
        }
    }
}
