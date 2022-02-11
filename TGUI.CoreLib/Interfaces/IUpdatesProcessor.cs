using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TGUI.CoreLib.Interfaces
{
    public interface IBotCore
    {
        public Task ProcessUpdate(Update update);
    }
}
