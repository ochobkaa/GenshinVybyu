using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Actions.Utils;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services
{
    public class ChatStateActions : IChatStateActions
    {
        private delegate ChatState StateModifier(ChatState curState);
        private delegate TOutput? ParamGetter<TOutput>(ChatState curState);

        private readonly IServiceProvider _provider;
        private readonly BotConfiguration _botConf;

        public ChatStateActions(IServiceProvider provider, IOptions<BotConfiguration> botConf)
        {
            _provider = provider;
            _botConf = botConf.Value;
        }

        private async Task<TOutput?> GetParam<TOutput>(ChatId chatId, ParamGetter<TOutput> getter)
        {
            var stateManager = _provider.GetService<IChatStateManager>();
            ChatState? state = await stateManager.GetState(chatId);

            if (state == null) return default;

            TOutput? output = getter(state);
            return output;
        }

        private async Task<bool> SetParam(ChatId chatId, StateModifier modifier)
        {
            var stateManager = _provider.GetService<IChatStateManager>();
            ChatState? state = await stateManager.GetState(chatId);

            bool success;
            if (state == null)
                state = new();

            ChatState newState = modifier(state);
            success = await stateManager.SetState(chatId, newState);

            return success;
        }

        private async Task<TOutput?> GetAndSetParam<TOutput>(
            ChatId chatId, 
            ParamGetter<TOutput> getter,
            StateModifier modifier
        )
        {
            var stateManager = _provider.GetService<IChatStateManager>();
            ChatState? state = await stateManager.GetState(chatId);

            if (state != null)
            {
                TOutput? output = getter(state);

                ChatState newState = modifier(state);
                await stateManager.SetState(chatId, newState);

                return output;
            }
            else
            {
                ChatState newState = new();
                newState = modifier(newState);
                await stateManager.SetState(chatId, newState);

                return default;
            }
        }

        public async Task<InputChainState?> GetInputChain(ChatId chatId)
        {
            InputChainState? Getter(ChatState state) => state.InputChain;

            InputChainState? inputChain = await GetParam(chatId, Getter);
            return inputChain;
        }

        public async Task<bool> StartInputChain(ChatId chatId, string inputChainName)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;

                var inputChain = new InputChainState()
                {
                    Name = inputChainName,
                    Step = 0
                };
                state.InputChain = inputChain;

                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }

        public async Task<bool> NextParam<TParam>(ChatId chatId, TParam param)
        {
            InputChainState ChainStateModifier(InputChainState chainState)
            {
                InputChainState newChainState = chainState;

                if (newChainState.InputCache == null)
                    newChainState.InputCache = new List<string>();

                string? paramStr = param?.ToString();
                if (!string.IsNullOrEmpty(paramStr))
                    newChainState.InputCache.Add(paramStr);

                newChainState.Step++;

                return newChainState;
            }

            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                InputChainState? chainState = state.InputChain;
                
                if (chainState != null)
                {
                    InputChainState newChainState = ChainStateModifier(chainState);
                    modified.InputChain = newChainState;
                }

                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }

        public async Task<IList<string>?> FinishInputChain(ChatId chatId)
        {
            IList<string>? Getter(ChatState state) => state.InputChain?.InputCache;

            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                state.InputChain = null;
                return modified;
            }

            IList<string>? inputCache = await GetAndSetParam(chatId, Getter, Modifier);

            return inputCache;
        }

        public async Task<bool> IsSuperUser(ChatId chatId)
        {
            bool Getter(ChatState state) => state.SuperUser ?? false;

            bool isSuperUser = await GetParam(chatId, Getter);
            return isSuperUser;
        }

        public async Task<bool> EnableSuperUser(ChatId chatId)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                modified.SuperUser = true;
                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }

        public async Task<bool> DisableSuperUser(ChatId chatId)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                modified.SuperUser = false;
                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }
    }
}
