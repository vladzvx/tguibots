using System;
using System.Linq;
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
            string text; 

            TextMessage mess;
            if (message.ForwardFrom != null || message.ForwardSenderName != null || message.ForwardFromChat != null || message.ForwardDate != null)
            {
                mess = new ForwardMessage()
                {
                    SourceChatId=message.Chat.Id,
                    SourceMessageId=message.MessageId,
                    botClient = botClient,
                    dataLogger = dataLogger,
                    TargetChatId = targetChat,
                    Text = "⬆️"+ appendix,
                    PostSendingAction = PostSendingAction
                };
            }
            else if (message.Photo != null && message.Photo.Length>0)
            {
                text = (message.Caption ?? string.Empty) + appendix;
                text = SupportFunctions.TextFormatingRecovering(message.CaptionEntities, text);
                mess = new PhotoMessage()
                {
                    botClient = botClient,
                    dataLogger = dataLogger,
                    TargetChatId = targetChat,
                    Text = text,
                    PostSendingAction = PostSendingAction,
                    FileId = message.Photo.Last().FileId
                };
            }
            else if (message.Video != null)
            {
                text = (message.Caption ?? string.Empty) + appendix;
                text = SupportFunctions.TextFormatingRecovering(message.CaptionEntities, text);
                mess = new VideoMessage()
                {
                    botClient = botClient,
                    dataLogger = dataLogger,
                    TargetChatId = targetChat,
                    Text = text,
                    PostSendingAction = PostSendingAction,
                    FileId = message.Video.FileId
                };
            }

            else
            {
                text = (message.Text ?? string.Empty) + appendix;
                text = SupportFunctions.TextFormatingRecovering(message.Entities, text);
                mess = new TextMessage()
                {
                    botClient = botClient,
                    dataLogger = dataLogger,
                    TargetChatId = targetChat,
                    Text = text,
                    PostSendingAction = PostSendingAction
                };
            }
            return mess;
        }
    }
}
