using CashboxCommands.Commands.System.Terminal;
using CashboxCommands.Converters;
using CashboxCommands.Data;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Server.Interfaces;
using Server.Logger;
using Server.Modules;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        CommandsDao _commandsDao;
        public ChatHub(CommandsDao commandsDao)
        {
            _commandsDao = commandsDao;
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($" User {Context.ConnectionId}  Connected ");
            return base.OnConnectedAsync();
        }
        private readonly StringToCommandConverter stringToCommandConverter = new StringToCommandConverter();
        public async void JoinGroup(string grupID)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, grupID);
            await base.OnConnectedAsync();
            Console.WriteLine(grupID + " JoinGroup ");
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($" User {Context.ConnectionId}  Left ");
            await base.OnDisconnectedAsync(exception);


        }

        public async void SendStringCommand(object Command)
        {

            var dateTime = DateTime.Now;
            try
            {
                Console.WriteLine("ReceivedCommand");
                Console.WriteLine(Command.ToString());
                Console.WriteLine();

                var com = stringToCommandConverter.ConvertedObject(Command.ToString());
                com.ServerDateTime = dateTime;
                //Command = JsonConvert.SerializeObject(com);

                Task.Factory.StartNew(() => _commandsDao.WriteCommands(com));


                await Clients.Group("Clients").SendAsync("ReceiveStringCommand", com);
            }
            catch (Exception ex)
            {
                Logger.Logger.WriteLog(ex, Command.ToString());

            }
        }

    }
}
