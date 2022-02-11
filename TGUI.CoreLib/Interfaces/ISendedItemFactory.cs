using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TGUI.CoreLib.Interfaces
{
    public interface ISendedItemFactory
    {
        public ISendedItem ReCreateMessage(Message message, long targetChat, bool addAutor, Func<Message, Task> PostSendingAction);
    }
}
