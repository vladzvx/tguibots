using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Models.Messages
{
    public class TextMessage : ISendedItem
    {
        public long TargetChatId { get; init; }
        public string Text { get; init; }
        public ITelegramBotClient botClient { get; init; }
        public IDataLogger dataLogger { get; init; }
        public Func<Message, Task> PostSendingAction { get; init; }
        public virtual async Task<Message> Send()
        {
            Message message = await botClient?.SendTextMessageAsync(TargetChatId, Text, Telegram.Bot.Types.Enums.ParseMode.Html);
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
