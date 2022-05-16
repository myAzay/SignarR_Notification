using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Interfaces
{
    public interface ILoggerWrapper<T> : ILogger<T> where T : class
    {
        void Info(string message);
        void Error(string message);
        void Error(Exception error, string message);
        void Warning(string message);
        void Debug(string message);
    }
}
