using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Services
{
    public class BotsEntryPoint : IHostedService
    {
        private readonly ITelegramBotClient botClient;
        public BotsEntryPoint(IServiceProvider serviceProvider)
        {
            this.botClient = (ITelegramBotClient)serviceProvider.GetService(typeof(ITelegramBotClient));
            MessageHandler.updatesProcessor = (IBotCore)serviceProvider.GetService(typeof(IBotCore));
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            botClient.StartReceiving<MessageHandler>();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
