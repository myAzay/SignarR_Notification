using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace SignalR.Client
{
    class Program
    {
        const string BASE_URL = @"http://localhost:5000";
        const string BASE_URL_HUB = BASE_URL + @"/notificationHub";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logfile.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Debug("Starting up");

            var connection = new HubConnectionBuilder()
                .WithUrl(BASE_URL_HUB)
                //.ConfigureLogging(logging => {
                //    logging.AddConsole();
                //    logging.SetMinimumLevel(LogLevel.Information);
                //    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Information);
                //    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Information);
                //})
                .WithAutomaticReconnect()
                .Build();

            connection.Closed += Connection_Closed;
            connection.Reconnecting += Connection_Reconnecting;
            connection.Reconnected += Connection_Reconnected;

            connection.StartAsync().Wait();
            connection.InvokeCoreAsync("SendNotification", args:new[] {"Test"});
            connection.On("ReceiveNotification", (string message) =>
             {
                 Console.WriteLine(message);
             }
            );

            Console.Read();
        }

        private static Task Connection_Reconnected(string arg)
        {
            Console.WriteLine("Connection Reconnected");
            return Task.CompletedTask;
        }

        private static Task Connection_Reconnecting(Exception arg)
        {
            Console.WriteLine("Connection Reconnecting");
            return Task.CompletedTask;
        }

        private static Task Connection_Closed(Exception arg)
        {
            Console.WriteLine("Connection Closed");
            return Task.CompletedTask;
        }
    }
}
