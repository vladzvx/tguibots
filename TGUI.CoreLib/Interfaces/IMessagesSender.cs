using System;
using System.Collections.Generic;
using System.Text;

namespace TGUI.CoreLib.Interfaces
{
    public interface IMessagesSender
    {
        public void AddItem(ISendedItem sendedItem);
    }
}
