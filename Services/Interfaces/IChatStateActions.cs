using GenshinVybyu.Actions.Utils;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IChatStateActions : IService
    {
        public Task<string?> GetCommandString(ChatId chatId);
        public Task<bool> SetCommandString(ChatId chatId, string command);
        public Task<bool> SetCommandString(ChatId chatId, string token, ActionArgs args);
        public Task<bool> AddArg(ChatId chatId, string argStr);
        public Task<bool> ClearCommandString(ChatId chatId);
        public Task<bool> IsSuperUser(ChatId chatId);
        public Task<bool> EnableSuperUser(ChatId chatId);
        public Task<bool> DisableSuperUser(ChatId chatId);
    }
}
