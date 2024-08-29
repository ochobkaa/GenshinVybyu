using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Actions.Utils;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services
{
    public class ChatStateActions : IChatStateActions
    {
        private readonly IServiceProvider _provider;
        private readonly BotConfiguration _botConf;

        public string ChainName => "chainname";
        public string ChainStep => "chainstep";
        public string Params => "params";
        public string SuperUser => "superuser";

        public ChatStateActions(IServiceProvider provider, IOptions<BotConfiguration> botConf)
        {
            _provider = provider;
            _botConf = botConf.Value;
        }

        public async Task<string?> GetInputChainName(ChatId chatId)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            string? chainName = await manager.GetState(chatId, ChainName, v => v);
            return chainName;
        }

        public async Task<int?> GetInputChainStep(ChatId chatId)
        {
            int IntParser(string v)
            {
                int.TryParse(v, out int vInt);
                return vInt;
            };

            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            int step = await manager.GetState(chatId, ChainStep, IntParser);
            return step;
        }

        public async Task<bool> StartInputChain(ChatId chatId, string chainName)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            bool nameSuccess = await manager.SetState(chatId, ChainName, chainName);
            bool stepSuccess = await manager.SetState(chatId, ChainStep, 0);
            bool success = nameSuccess && stepSuccess;

            return success;
        }

        public async Task<bool> NextParam(ChatId chatId, string param)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            string? chainParams = await manager.GetState(chatId, Params, v => v);

            bool success;
            if (chainParams is not null)
            {
                string newParams = $"{chainParams} {param}";
                success = await manager.SetState(chatId, Params, newParams);
            }
            else
            {
                success = await manager.SetState(chatId, Params, param);
            }

            return success;
        }

        public async Task<bool> NextChainAction(ChatId chatId)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            int? step = await GetInputChainStep(chatId);

            if (step is not null)
            {
                step++;

                bool success = await manager.SetState(chatId, ChainStep, step);
                return success;
            }
            else
                return false;
        }

        public async Task<bool> ClearChatCache(ChatId chatId)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            bool nameSuccess = await manager.DeleteState(chatId, ChainName);
            bool stepSuccess = await manager.DeleteState(chatId, ChainStep);
            bool paramsSuccess = await manager.DeleteState(chatId, Params);
            bool success = nameSuccess && stepSuccess && paramsSuccess;

            return success;
        }

        public async Task<string[]?> FinishInputChain(ChatId chatId)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();


            string? argsStr = await manager.GetState(chatId, Params, v => v);
            if (argsStr is null)
                return null;

            string[] args = argsStr.Split(" ");

            bool clearSuccess = await ClearChatCache(chatId);

            return args;
        }

        public async Task<bool> IsSuperUser(ChatId chatId)
        {
            bool BoolParser(string v)
            {
                bool.TryParse(v, out bool vBool);
                return vBool;
            };

            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            bool? isSuperUser = await manager.GetState(chatId, SuperUser, BoolParser);
            return isSuperUser ?? false;
        }

        public async Task<bool> EnableSuperUser(ChatId chatId)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            bool success = await manager.SetState(chatId, SuperUser, true);
            return success;
        }

        public async Task<bool> DisableSuperUser(ChatId chatId)
        {
            IChatStateManager manager = _provider.GetRequiredService<IChatStateManager>();

            bool success = await manager.SetState(chatId, SuperUser, false);
            return success;
        }
    }
}
