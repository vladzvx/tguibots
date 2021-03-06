using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TGUI.CoreLib.Models.Messages
{
    public class ForwardMessage : TextMessage
    {
        public long SourceChatId { get; set; }
        public int SourceMessageId { get; set; }
        public override async Task<Message> Send()
        {
            Message message = await botClient?.ForwardMessageAsync(TargetChatId, SourceChatId, SourceMessageId);
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
