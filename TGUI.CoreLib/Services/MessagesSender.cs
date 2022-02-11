using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Services
{
    public class MessagesSender : IMessagesSender
    {
        public void AddItem(ISendedItem sendedItem)
        {
            sendedItem.Send().Wait();
        }
    }
}
