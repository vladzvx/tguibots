using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;
using TGUI.CoreLib.Models.Messages;
using TGUI.CoreLib.Utils;

namespace TGUI.CoreLib.Services
{
    public class SendedItemFactory : ISendedItemFactory
    {
        private readonly ITelegramBotClient botClient;
        private readonly IDataLogger dataLogger;
        public SendedItemFactory(ITelegramBotClient botClient, IDataLogger dataLogger)
        {
            this.dataLogger = dataLogger;
            this.botClient = botClient;
        }
        public ISendedItem ReCreateMessage(Message message, long targetChat, bool addAutor, Func<Message, Task> PostSendingAction)
        {
            string appendix = string.Empty;
            if (addAutor && message.From != null)
            {
                appendix = string.Format("\n\n#id{0}\n<a href =\"tg://user?id={0}\">{1}</a>", message.From.Id, message.From.FirstName ?? string.Empty);
            }
            string text = (message.Text ?? message.Caption ?? string.Empty) + appendix;

            text = SupportFunctions.TextFormatingRecovering(message.Entities, text);
            TextMessage mess = new TextMessage()
            {
                botClient = botClient,
                dataLogger = dataLogger,
                TargetChatId = targetChat,
                Text = text,
                PostSendingAction = PostSendingAction
            };
            return mess;
        }
    }
}
