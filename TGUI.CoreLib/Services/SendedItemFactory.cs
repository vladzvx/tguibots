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
        private static string CreateAppendix(Message message, bool addAutor)
        {
            return message.From != null && addAutor ? string.Format("\n\n#id{0}\n<a href =\"tg://user?id={0}\">{1}</a>", message.From.Id, message.From.FirstName ?? string.Empty) : string.Empty;
        }
        private static string ExtractText(Message message, bool addAutor)
        {
            string text;
            string appendix = CreateAppendix(message, addAutor);

            if (message.ForwardFrom != null || message.ForwardSenderName != null || message.ForwardFromChat != null || message.ForwardDate != null)
            {
                text = "⬆️" + appendix;
            }
            else if (!string.IsNullOrEmpty(message.Text))
            {
                text = SupportFunctions.TextFormatingRecovering(message.Entities, (message.Text ?? string.Empty) + appendix);
            }
            else if (!string.IsNullOrEmpty(message.Caption))
            {
                text = SupportFunctions.TextFormatingRecovering(message.CaptionEntities, (message.Caption ?? string.Empty) + appendix);
            }
            else
            {
                text = string.Empty;
            }

            return text;
        }
        private TMessage CreateNew<TMessage>(long targetChatId, string text, Func<Message, Task> postSendingAction, Action<TMessage> additionaAction = null) where TMessage : TextMessage, new()
        {
            TMessage res = new TMessage()
            {
                botClient = botClient,
                dataLogger = dataLogger,
                TargetChatId = targetChatId,
                Text = text,
                PostSendingAction = postSendingAction
            };
            if (additionaAction != null)
            {
                additionaAction(res);
            }

            return res;
        }
        public ISendedItem ReCreateMessage(Message message, long targetChat, bool addAutor, Func<Message, Task> PostSendingAction)
        {
            ISendedItem resultMessage;
            string text = ExtractText(message, addAutor);
            if (message.ForwardFrom != null || message.ForwardSenderName != null || message.ForwardFromChat != null || message.ForwardDate != null)
            {
                resultMessage = CreateNew<ForwardMessage>(targetChat, text, PostSendingAction, item =>
                {
                    item.SourceChatId = message.Chat.Id;
                    item.SourceMessageId = message.MessageId;
                });
            }
            else
            {
                if (message.Photo != null && message.Photo.Length > 0)
                {
                    resultMessage = CreateNew<PhotoMessage>(targetChat, text, PostSendingAction, item => item.FileId = message.Photo.Last().FileId);
                }
                else if (message.Video != null)
                {
                    resultMessage = CreateNew<VideoMessage>(targetChat, text, PostSendingAction, item => item.FileId = message.Video.FileId);
                }
                else
                {
                    resultMessage = CreateNew<TextMessage>(targetChat, text, PostSendingAction);
                }
            }
            return resultMessage;
        }
    }
}
