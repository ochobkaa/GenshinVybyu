using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Types;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IChatStateActions : IService
    {
        public Task<InputChainState?> GetInputChain(ChatId chatId);
        public Task<bool> StartInputChain(ChatId chatId, string inputChainName);
        public Task<bool> NextParam<TParam>(ChatId chatId, TParam param);
        public Task<IList<string>?> FinishInputChain(ChatId chatId);
        public Task<bool> ClearChatCache(ChatId chatId);
        public Task<bool> IsSuperUser(ChatId chatId);
        public Task<bool> EnableSuperUser(ChatId chatId);
        public Task<bool> DisableSuperUser(ChatId chatId);
    }
}
