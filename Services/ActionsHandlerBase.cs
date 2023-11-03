using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Types;
using Telegram.Bot.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public abstract class ActionsHandlerBase<T> : IActionsHandler<T>
    {
        private readonly IActionsCollection _actions;
        private readonly IActionExecutor _executor;
        private readonly IChatStateActions _state;
        private readonly ICommandParser _parser;
        private readonly BotConfiguration _conf;
        private readonly ILogger _logger;

        public ActionsHandlerBase(
            IActionsCollection actions,
            IActionExecutor executor,
            IChatStateActions state,
            ICommandParser parser, 
            IOptions<BotConfiguration> conf,
            ILogger logger
        )
        {
            _actions = actions;
            _executor = executor;
            _state = state;
            _parser = parser;
            _conf = conf.Value;
            _logger = logger;
        }

        public abstract Task Handle(T obj, CancellationToken cancellationToken);

        private ParsedCommand? GetChainCommand(InputChainState chainState)
        {
            string chainName = chainState.Name;
            int chainStep = chainState.Step;
            string cPref = _conf.CommandPrefix;
            string chainCommand = $"{cPref}{chainName} {chainStep}";

            ParsedCommand? command = _parser.ParseText(chainCommand);
            return command;
        }

        protected async Task HandleAction(ChatId chatId, ParsedCommand command, CancellationToken cancellationToken)
        {
            IBotAction? action = _actions.GetAction(command);
            if (action == null) return;

            ActionArgs args = command.Args;
            await _executor.Exec(chatId, action.Run, args, cancellationToken);
        }

        protected async Task HandleWithText(ChatId chatId, string mText, CancellationToken cancellationToken)
        {
            InputChainState? chainState = await _state.GetInputChain(chatId);

            ParsedCommand? command = _parser.ParseText(mText);

            if (command == null && chainState != null)
            {
                command = GetChainCommand(chainState);

                await _state.NextParam(chatId, mText);
            }

            if (command != null)
                await HandleAction(chatId, command, cancellationToken);
        }
    }
}
