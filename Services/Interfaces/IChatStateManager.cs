using GenshinVybyu.Types;
using Telegram.Bot.Types;
using static GenshinVybyu.Services.ChatStateManager;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IChatStateManager : IService
    {
        public Task<T?> GetState<T>(ChatId chatId, string paramName,
            StrToValConverter<T?> conv);

        public Task<bool> SetState<T>(ChatId chatId, string paramName, T val,
            ValToStrConverter<T>? conv = null);

        public Task<bool> DeleteState(ChatId chatId, string paramName);
    }
}
