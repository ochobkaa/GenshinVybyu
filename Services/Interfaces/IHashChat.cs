using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IHashChat : IService
    {
        public string Hash(ChatId chatId);
    }
}
