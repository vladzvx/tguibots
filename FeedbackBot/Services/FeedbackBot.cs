using FeedbackBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;
using TGUI.CoreLib.Services;
using TGUI.CoreLib.Utils;

namespace FeedbackBot.Services
{
    public class FeedbackBotCore : BotCoreBase
    {
        public Profile Profile;
        public FeedbackBotCore(IMessagesSender messagesSender, IDataLogger messagesLogger, ITelegramBotClient botClient, ISendedItemFactory sendedItemFactory) : base(messagesSender, messagesLogger, botClient, sendedItemFactory)
        {
            var profiles = messagesLogger.GetData<Profile>(item => item.BotId==this.Id).Result;
            if (profiles != null && profiles.Count > 0)
            {
                long maxTime = profiles.Max(item => item.Timestamp.Ticks);
                Profile = profiles.Find(item => item.Timestamp.Ticks==maxTime);
            }
            else
            {
                Profile = new Profile()
                {
                    BotId = this.Id,
                
                };
            }
        }

        public override async Task ProcessPrivateMessage(Message message)
        {
            if (SupportFunctions.TryParseCommand(message.Text,out var res))
            {
               
            }
            else
            {
                var mess = sendedItemFactory.ReCreateMessage(message, Profile.TargetChat, true,async (mess)=> 
                {
                    Link link = new Link()
                    {
                        ExternalChatId=message.Chat.Id,
                        ExternalMessageId = message.MessageId,
                        InternalChatId=mess.Chat.Id,
                        InternalMessageId=mess.MessageId
                    };
                    await messagesLogger.Log(link);
                });
                messagesSender.AddItem(mess);
            }
        }

        public override async Task ProcessGroupMessage(Message message)
        {
            if (SupportFunctions.TryParseCommand(message.Text, out var res))
            {
                if (res.command != null)
                {
                    if (res.command.ToLower().Equals(SupportFunctions.Activate))
                    {
                        if (res.content != null && res.content.Equals(this.Token))
                        {
                            Profile = new Profile()
                            {
                                BotId = this.Id,
                                TargetChat = message.Chat.Id
                            };
                            await messagesLogger.Log(Profile, item => item.BotId == Profile.BotId);
                        }
                    }
                    else if (res.command.ToLower().Equals(SupportFunctions.Ban) && message.ReplyToMessage != null)
                    {
                        //TODO механику бана
                    }
                }
            }
            else if (message.ReplyToMessage!=null)
            {
                List<Link> links = await messagesLogger.GetData<Link>((item) => item.InternalChatId == message.ReplyToMessage.Chat.Id && item.InternalMessageId == message.ReplyToMessage.MessageId);
                if (links.Count == 1)
                {
                    var mess = sendedItemFactory.ReCreateMessage(message, links[0].ExternalChatId, false, async (mess) =>
                    {
                        Link link = new Link()
                        {
                            ExternalChatId = message.Chat.Id,
                            ExternalMessageId = message.MessageId,
                            InternalChatId = mess.Chat.Id,
                            InternalMessageId = mess.MessageId
                        };
                        await messagesLogger.Log(link);
                    });
                    messagesSender.AddItem(mess);
                }
            }
        }
    }
}
