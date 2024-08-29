using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Types;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IChatStateActions : IService
    {
        public Task<string?> GetInputChainName(ChatId chatId);
        public Task<int?> GetInputChainStep(ChatId chatId);
        public Task<bool> StartInputChain(ChatId chatId, string inputChainName);
        public Task<bool> NextParam(ChatId chatId, string param);
        public Task<bool> NextChainAction(ChatId chatId);
        public Task<string[]?> FinishInputChain(ChatId chatId);
        public Task<bool> ClearChatCache(ChatId chatId);
        public Task<bool> IsSuperUser(ChatId chatId);
        public Task<bool> EnableSuperUser(ChatId chatId);
        public Task<bool> DisableSuperUser(ChatId chatId);
    }
}
