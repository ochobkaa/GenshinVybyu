using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IMessagesStore : IService
    {
        public BotMessage MessageByName(string name);
    }
}
