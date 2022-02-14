using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Hubs
{
    public class LoggingHub : Hub<ITypedLogging>
    {
        public Task SendInfoMessage(string message)
        {
            return Clients.Caller.GetInfoLog(message);
        }
        public Task SendWarningMessage(string message)
        {
            return Clients.Caller.GetWarningLog(message);
        }
        public Task SendError(Exception exception, string message)
        {
            var stackTrace = exception.StackTrace;
            return Clients.Caller.GetErrorLog(stackTrace, message);
        }
        public Task SendErrorMessage(string message)
        {
            return Clients.Caller.GetErrorMessageLog(message);
        }
        public Task SendDebugMessage(string message)
        {
            return Clients.Caller.GetDebugLog(message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
