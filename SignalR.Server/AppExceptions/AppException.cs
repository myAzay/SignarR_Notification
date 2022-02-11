using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.AppExceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"application/json";
        public AppException() : base() { }

        public AppException(string statusCode, string message) : base(message)
        {
            StatusCode = int.Parse(statusCode);
        }
        public AppException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public AppException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public AppException(int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }
    }
}
