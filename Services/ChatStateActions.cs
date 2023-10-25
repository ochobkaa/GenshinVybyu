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

        public async Task<string?> GetCommandString(ChatId chatId)
        {
            string? Getter(ChatState state) => state.Command;

            string? command = await GetParam(chatId, Getter);
            return command;
        }

        public async Task<bool> SetCommandString(ChatId chatId, string command)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                modified.Command = command;
                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }

        public async Task<bool> SetCommandString(ChatId chatId, string token, ActionArgs args)
        {
            var command = new ParsedCommand()
            {
                Token = token,
                Args = args,
                CommandPrefix = _botConf.CommandPrefix,
                KeyAttrValuePrefix = _botConf.KeyAttrValuePrefix
            };
            string commandStr = command.ToString();

            bool success = await SetCommandString(chatId, commandStr);
            return success;
        }

        public async Task<bool> AddArg(ChatId chatId, string argStr)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;

                string? command = state.Command;
                string newCommand = $"{command} {argStr}";
                modified.Command = newCommand;

                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }

        public async Task<bool> ClearCommandString(ChatId chatId)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                modified.Command = null;
                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }

        public async Task<bool> IsSuperUser(ChatId chatId)
        {
            bool Getter(ChatState state) => state.SuperUser == "true";

            bool isSuperUser = await GetParam(chatId, Getter);
            return isSuperUser;
        }

        public async Task<bool> EnableSuperUser(ChatId chatId)
        {
            ChatState Modifier(ChatState state)
            {
                ChatState modified = state;
                modified.SuperUser = "true";
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
                modified.SuperUser = "false";
                return modified;
            }

            bool success = await SetParam(chatId, Modifier);
            return success;
        }
    }
}
