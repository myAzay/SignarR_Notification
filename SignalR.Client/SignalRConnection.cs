using Microsoft.AspNetCore.SignalR.Client;
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
            connection.On("GetInfoLog", (string message) =>
                {
                    Program._logger.LogInformation(message);
                }
            );
            connection.On("GetWarningLog", (string message) =>
                {
                    Program._logger.LogWarning(message);
                }
            );
            connection.On("GetErrorLog", (Exception exception, string message) =>
                {
                    Program._logger.LogError(exception, message);
                }
            );
            connection.On("GetErrorMessageLog", (string message) =>
                {
                    Program._logger.LogError(message);
                }
            );
            connection.On("GetDebugLog", (string message) =>
                {
                    Program._logger.LogDebug(message);
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
                    Program._logger.LogError(error, "Failed to connect to Hub");
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
            await connection.InvokeCoreAsync("SendInfoMessage", args: new[] { notification });
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
            ConnectionToHub();
            return Task.CompletedTask;
        }

        private Task Connection_Reconnected(string arg)
        {
            Program._logger.LogWarning("Connection to Hub has been reconnected");
            return Task.CompletedTask;
        }
    }
}
