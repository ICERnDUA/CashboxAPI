using CashboxCommands.Data;
using Client.Events;
using Client.Interfaces;
using Client.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.SignalR
{
    public class ConnectedManager
    {
        IEventAggregator _eventAggregator;
        private HubConnection connection;
        private readonly INetworkDAO _networkDAO;
        public ConnectedManager()
        {
            _networkDAO = ContainerLocator.Container.Resolve<INetworkDAO>();
            
            _eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>(); 
           
            
        }
        public void InitConnection()
        {
            try
            {
                var networkCon = _networkDAO.ReadNetworkData();
                
                string url = $"http://{networkCon.IpAddress}:{networkCon.Port}/CashboxHub";
                connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();
                Signal();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing connection: {ex.Message}");
            }
          
        }
        private async Task Signal()
        { 
            connection.On<object>("ReceiveStringCommand", (message) =>
            {
                
                _eventAggregator.GetEvent<SendStringJsonEvent>().Publish(message);
            });           

            connection.Closed += async error =>
            {
                Console.WriteLine("Connection closed.");
                await Task.Delay(10000);
                await connection.StartAsync();
                await connection.InvokeAsync("JoinGroup", "Clients");
            };

            await connection.StartAsync();
            await connection.InvokeAsync("JoinGroup", "Clients"); 
        }

        public async Task StopConnection()
        {
            await connection.StopAsync();
        }
        public async Task StartConnection()
        {
            await connection.StartAsync();
            await connection.InvokeAsync("JoinGroup", "Clients");
        }
    }
}
