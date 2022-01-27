using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace SignalR.Client
{
    class Program
    {
        const string BASE_URL = @"http://localhost:5000";
        const string BASE_URL_CHAT = BASE_URL + @"/notificationHub";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var connection = new HubConnectionBuilder()
                .WithUrl(BASE_URL_CHAT)
                .Build();

            connection.StartAsync().Wait();
            connection.InvokeCoreAsync("SendNotification", args:new[] {"Test"});
            connection.On("ReceiveNotification", (string message) =>
             {
                 Console.WriteLine(message);
             }
            );

            Console.Read();
        }
    }
}
