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
    public class LoggerWrapper<T> : ILoggerWrapper<T> where T : class
    {
        private readonly IHubContext<LoggingHub, ITypedLogging> _notificationHubContext;
        private readonly ILogger<T> _logger;
        public LoggerWrapper(IHubContext<LoggingHub, ITypedLogging> notificationHubContext,
            ILogger<T> logger)
        {
            _logger = logger;
            _notificationHubContext = notificationHubContext;
        }

        private void SendLog(string message, LogLevel logLevel, Exception exception = null)
        {
            switch (logLevel)
            {
                case LogLevel.Warning:
                    _notificationHubContext.Clients.All.GetWarningLog(message);
                    break;
                case LogLevel.Error:
                    if (exception is null)
                        _notificationHubContext.Clients.All.GetErrorMessageLog(message);
                    else
                        _notificationHubContext.Clients.All.GetErrorLog(exception.StackTrace, message);
                    break;
                case LogLevel.Debug:
                    _notificationHubContext.Clients.All.GetDebugLog(message);
                    break;
                default:
                    _notificationHubContext.Clients.All.GetInfoLog(message);
                    break;
            }
        }
        private string ValidateLogString(string message)
        {
            return $"{DateTime.Now} : {message}";
        }
        public void Info(string message)
        {
            var logMessage = ValidateLogString(message);
            _logger.LogInformation(logMessage);
            SendLog(logMessage, LogLevel.Info);
        }

        public void Error(string message)
        {
            var logMessage = ValidateLogString(message);
            _logger.LogError(logMessage);
            SendLog(logMessage, LogLevel.Error);
        }

        public void Error(Exception error, string message)
        {
            var logMessage = ValidateLogString(message);
            _logger.LogError(error, logMessage);
            SendLog(logMessage, LogLevel.Error, error);
        }
        public void Warning(string message)
        {
            var logMessage = ValidateLogString(message);
            _logger.LogWarning(logMessage);
            SendLog(logMessage, LogLevel.Warning);
        }
        public void Debug(string message)
        {
            var logMessage = ValidateLogString(message);
            _logger.LogDebug(logMessage);
            SendLog(logMessage, LogLevel.Debug);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
