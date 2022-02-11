﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SignalR.Client
{
    public class SignalRConnection
    {
        private readonly HubConnection connection;
        private const int DEFAULT_RECONNECT_COUNT = 4;
        private int ReconnectTriesCountLeft = DEFAULT_RECONNECT_COUNT;
        public SignalRConnection(string url)
        {
            connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            connection.Closed += Connection_Closed;
            connection.Reconnecting += Connection_Reconnecting;
            connection.Reconnected += Connection_Reconnected;

            ConfigureHandles();
        }
        private void ConfigureHandles()
        {
            connection.On("ReceiveNotification", (string message) =>
            {
                Console.WriteLine(message);
            }
           );
        }
        public void ConnectionToHub()
        {
            while(ReconnectTriesCountLeft > 0)
            {
                try
                {
                    connection.StartAsync().Wait();
                    break;
                }
                catch (Exception error)
                {
                    Program._logger.LogError("Failed to connect to Hub");
                    ReconnectTriesCountLeft--;
                    ConnectionToHub();
                    connection.StopAsync().GetAwaiter().GetResult();
                    Program._logger.LogCritical("Can't connect to hub");
                    Environment.Exit(-1);
                    Console.Read();
                }
            }
        }

        public async Task TestSignalHub(string notification)
        {
            await connection.InvokeCoreAsync("SendNotification", args: new[] { notification });
        }
        private Task Connection_Closed(Exception arg)
        {
            Program._logger.LogWarning("Connection to Hub has been closed");
            return Task.CompletedTask;
        }

        private Task Connection_Reconnecting(Exception arg)
        {
            Program._logger.LogWarning("Connection to Hub has been lost, reconnecting....");
            ReconnectTriesCountLeft = DEFAULT_RECONNECT_COUNT;
            //Task.Run(new Action(ConnectionToHub)).GetAwaiter().GetResult();
            ConnectionToHub();
            //connection.StartAsync().GetAwaiter().GetResult();
            return Task.CompletedTask;
        }

        private Task Connection_Reconnected(string arg)
        {
            Program._logger.LogWarning("Connection to Hub has been reconnected");
            return Task.CompletedTask;
        }
    }
}