using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SignalR.Client
{
    class Program
    {
        const string BASE_URL = @"http://localhost:5000";
        const string BASE_URL_HUB = BASE_URL + @"/notificationHub";
        public static ILogger<Program> _logger;
        static void Main(string[] args)
        {
            var serviceCollection = SetupStaticLogger();
            var logger = serviceCollection.GetService<ILogger<Program>>();
            _logger = logger;
            _logger.LogInformation("Start up");

            var connection = new SignalRConnection(BASE_URL_HUB);

            _logger.LogInformation("Getting connection to Hub...");
            connection.ConnectionToHub();
            _logger.LogInformation("Connected to Hub");

            connection.TestSignalHub("Test").GetAwaiter().GetResult();
            Console.Read();
        }
        private static ServiceProvider SetupStaticLogger()
        {
            var configuration = new ConfigurationBuilder()
                .Build();
            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "logs/logfile.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true, buffered: true)
                .CreateLogger();

            var serviceCollection = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog(Log.Logger))
                .BuildServiceProvider();

            return serviceCollection;
        }
    }
}
