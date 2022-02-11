using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SignalR.Server.AppExceptions;
using SignalR.Server.Helper;
using SignalR.Server.Hubs;
using SignalR.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SignalR.Server.Middlewares
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHubContext<LoggingHub, ITypedLogging> _notificationHubContext;
        private readonly ILogger<AppExceptionMiddleware> _logger;
        public AppExceptionMiddleware(RequestDelegate next, ILogger<AppExceptionMiddleware> logger,
            IHubContext<LoggingHub, ITypedLogging> notificationHubContext)
        {
            _next = next;
            _notificationHubContext = notificationHubContext;
            _logger = logger;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var errorHandler = new ErrorHandler();
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException appException:
                        _logger.Error(error, appException.Message, _notificationHubContext);
                        response.StatusCode = appException.StatusCode;
                        response.ContentType = appException.ContentType;
                        errorHandler = new ErrorHandler(appException.StatusCode, appException.Message);
                        break;
                    default:
                        _logger.Error(error, error.Message, _notificationHubContext);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorHandler = new ErrorHandler(response.StatusCode, error.Message);
                        break;
                }

                await response.WriteAsync(JsonConvert.SerializeObject(errorHandler));
            }
        }
    }
}
