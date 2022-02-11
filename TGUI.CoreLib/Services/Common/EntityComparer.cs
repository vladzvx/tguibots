using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TGUI.CoreLib.Services.Common
{
    public class EntityComparer : IComparer<MessageEntity>
    {
        int IComparer<MessageEntity>.Compare(MessageEntity x, MessageEntity y)
        {
            if (x.Offset > y.Offset)
            {
                return 1;
            }
            else if (x.Offset < y.Offset)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
