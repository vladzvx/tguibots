using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Models.Messages
{
    public class ForwardMessage : TextMessage
    {
        public long SourceChatId { get; init; }
        public int SourceMessageId { get; init; }
        public override async Task<Message> Send()
        {
            var message = await botClient?.ForwardMessageAsync(TargetChatId, SourceChatId, SourceMessageId);
            if (message != null)
            {
                if (PostSendingAction != null)
                {
                    await PostSendingAction(message);
                }
                await dataLogger?.Log(message);
            }
            await Task.Delay(2000);
            await base.Send();

            return message;
        }
    }
}
