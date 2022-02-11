using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TGUI.CoreLib.Interfaces
{
    public interface ISendedItem
    {
        public Func<Message, Task> PostSendingAction { get; init; }
        public Task<Message> Send();
    }
}
