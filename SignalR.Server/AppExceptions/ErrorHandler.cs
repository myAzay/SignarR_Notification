using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.AppExceptions
{
    public class ErrorHandler
    {
        public int StatusCode { get; }
        public string Message { get; }

        public ErrorHandler(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ErrorHandler()
        {
        }
    }
}
