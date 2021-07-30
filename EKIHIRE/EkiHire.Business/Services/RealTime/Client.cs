using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace EkiHire.Business.Services
{
    public class ChatClient
    {
        HubConnection connection;
        public ChatClient()
        {
            
        }
        public async Task TestRealTimeFunc()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44300/ChatHub")
                //.WithUrl(AppDomain.CurrentDomain)
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.On<string>("ReceiveNotification", (message) =>
            {
                System.Diagnostics.Debug.WriteLine($"working :: {message}");
                Console.WriteLine("this works");
            });
            await connection.StartAsync();
            while(connection.State != HubConnectionState.Connected) {  }
            await connection.InvokeAsync("SendNotification", "just testing");

        }
    }
}
