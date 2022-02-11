using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Interfaces
{
    public interface ITypedLogging
    {
        Task GetErrorLog(Exception exception, string message);
        Task GetErrorMessageLog(string message);
        Task GetInfoLog(string message);
        Task GetWarningLog(string message);
    }
}
