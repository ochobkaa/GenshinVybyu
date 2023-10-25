using GenshinVybyu.Types;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IChatStateManager : IService
    {
        public Task<ChatState?> GetState(ChatId chatId);
        public Task<bool> SetState(ChatId chatId, ChatState state);
    }
}
