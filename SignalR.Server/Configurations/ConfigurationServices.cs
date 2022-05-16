using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Server.Helper;
using SignalR.Server.Interfaces;
using SignalR.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Server.Configurations
{
    public static class ConfigurationServices
    {
        public static void Config(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient(typeof(ILoggerWrapper<>), typeof(LoggerWrapper<>));
        }
    }
}
