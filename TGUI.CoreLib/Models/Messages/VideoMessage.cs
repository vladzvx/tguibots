using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Models.Messages
{
    public class VideoMessage : TextMessage
    {
        public string FileId { get; init; }
        public override async Task<Message> Send()
        {
            var message = await botClient?.SendVideoAsync(TargetChatId, new InputOnlineFile(FileId), caption:Text, parseMode: ParseMode.Html);
            if (message != null)
            {
                if (PostSendingAction != null)
                {
                    await PostSendingAction(message);
                }
                await dataLogger?.Log(message);
            }
            return message;
        }
    }
}
