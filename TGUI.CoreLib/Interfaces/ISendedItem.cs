using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TGUI.CoreLib.Enums;

namespace TGUI.CoreLib.Interfaces
{
    public interface ISendedItem
    {
        public Func<Message,Task> PostSendingAction { get; init; }
        public Task<Message> Send();
    }
}
