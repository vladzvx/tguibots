using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Services
{
    public class MessageHandler : IUpdateHandler
    {
        public static IBotCore updatesProcessor;
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await updatesProcessor?.ProcessUpdate(update);
        }
    }
}
