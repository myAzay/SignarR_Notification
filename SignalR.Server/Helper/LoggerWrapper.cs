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
            var logMessage = ValidateLogString(message);
            switch (logLevel)
            {
                case LogLevel.Warning:
                    _logger.LogWarning(logMessage);
                    _notificationHubContext.Clients.All.GetWarningLog(logMessage);
                    break;
                case LogLevel.Error:
                    if (exception is null)
                    {
                        _logger.LogError(logMessage);
                        _notificationHubContext.Clients.All.GetErrorMessageLog(logMessage);
                    }
                    else
                    {
                        _logger.LogError(exception, logMessage);
                        _notificationHubContext.Clients.All.GetErrorLog(exception.StackTrace, logMessage);
                    }
                    break;
                case LogLevel.Debug:
                    _logger.LogDebug(logMessage);
                    _notificationHubContext.Clients.All.GetDebugLog(logMessage);
                    break;
                default:
                    _logger.LogInformation(logMessage);
                    _notificationHubContext.Clients.All.GetInfoLog(logMessage);
                    break;
            }
        }
        private string ValidateLogString(string message)
        {
            return $"{DateTime.Now} : {message}";
        }
        public void Info(string message)
        {
            SendLog(message, LogLevel.Info);
        }

        public void Error(string message)
        {
            SendLog(message, LogLevel.Error);
        }

        public void Error(Exception error, string message)
        {
            SendLog(message, LogLevel.Error, error);
        }
        public void Warning(string message)
        {
            SendLog(message, LogLevel.Warning);
        }
        public void Debug(string message)
        {
            SendLog(message, LogLevel.Debug);
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
