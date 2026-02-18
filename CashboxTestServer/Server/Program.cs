using CashboxCommands.Converters;
using Microsoft.Extensions.DependencyInjection;
using Server.Hubs;
using Server.Interfaces;
using Server.Logger;
using Server.Modules;




namespace Server
{
    public class Program
    {
        static async Task Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            builder.Services.AddSingleton<INetworkDAO, NetworkDAO>();
            builder.Services.AddSingleton<CommandsDao>();

            var app = builder.Build();

            app.MapHub<ChatHub>("/CashboxHub");
            var com = app.Services.GetRequiredService<INetworkDAO>();
            var networkData = com.ReadNetworkData();
            if (networkData == null)
            {
                app.Run();
            }
            else
            {
                string url = $"http://{networkData.IpAddress}:{networkData.Port}";
                app.Run(url);
            }

        }
    }
}

