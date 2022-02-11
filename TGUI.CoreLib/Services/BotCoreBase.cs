using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Services
{
    public abstract class BotCoreBase : IBotCore
    {
        protected readonly long Id;
        protected readonly string Token;
        protected readonly IMessagesSender messagesSender;
        protected readonly ISendedItemFactory sendedItemFactory;
        protected readonly IDataLogger messagesLogger;
        public BotCoreBase(IMessagesSender messagesSender, IDataLogger messagesLogger, ITelegramBotClient botClient, ISendedItemFactory sendedItemFactory)
        {
            Token = Environment.GetEnvironmentVariable("TOKEN");
            if (botClient.BotId.HasValue)
            {
                Id = botClient.BotId.Value;
            }
            else
            {
                throw new ApplicationException("Bot id is null!");
            }

            this.messagesSender = messagesSender;
            this.messagesLogger = messagesLogger;
            this.sendedItemFactory = sendedItemFactory;
        }

        public virtual async Task ProcessPrivateMessage(Message message)
        {

        }

        public virtual async Task ProcessGroupMessage(Message message)
        {

        }

        public virtual async Task ProcessButtonClick(CallbackQuery update)
        {

        }

        public virtual async Task ProcessAddingToGroup(ChatMemberUpdated update)
        {

        }
        public async Task ProcessUpdate(Update update)
        {
            try
            {
                switch (update.Type)
                {
                    case Telegram.Bot.Types.Enums.UpdateType.MyChatMember:
                        {
                            if (update.MyChatMember != null)
                            {
                                await messagesLogger.Log(update.MyChatMember);
                                await ProcessAddingToGroup(update.MyChatMember);
                            }
                            break;
                        }
                    case Telegram.Bot.Types.Enums.UpdateType.Message:
                        {
                            if (update.Message != null)
                            {
                                await messagesLogger.Log(update.Message);
                                if (update.Message.From != null)
                                {
                                    TGUI.CoreLib.Models.User user = new TGUI.CoreLib.Models.User() { Id = update.Message.From.Id, Name = update.Message.From.FirstName, Username = update.Message.From.Username };
                                    await messagesLogger.Log(user, item => item.Id == user.Id);
                                }

                                if (update.Message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
                                {
                                    await ProcessPrivateMessage(update.Message);
                                }
                                else if (update.Message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Group || update.Message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Supergroup)
                                {
                                    await ProcessGroupMessage(update.Message);
                                }
                            }
                            break;
                        }
                    case Telegram.Bot.Types.Enums.UpdateType.CallbackQuery:
                        {
                            if (update.CallbackQuery != null)
                            {
                                await messagesLogger.Log(update.CallbackQuery);
                                await ProcessButtonClick(update.CallbackQuery);
                            }

                            break;
                        }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
