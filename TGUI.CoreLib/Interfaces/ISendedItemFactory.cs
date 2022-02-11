using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TGUI.CoreLib.Interfaces
{
    public interface ISendedItemFactory
    {
        public ISendedItem ReCreateMessage(Message message, long targetChat, bool addAutor, Func<Message, Task> PostSendingAction);
    }
}
