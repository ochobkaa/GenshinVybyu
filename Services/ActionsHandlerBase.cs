using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services
{
    public abstract class ActionsHandlerBase<T> : IActionsHandler<T>
    {
        private readonly IActionsCollection _actions;
        private readonly IActionExecutor _executor;
        private readonly IChatStateActions _state;
        private readonly ICommandParser _parser;
        private readonly ILogger _logger;

        public ActionsHandlerBase(
            IActionsCollection actions,
            IActionExecutor executor,
            IChatStateActions state,
            ICommandParser parser, 
            ILogger logger
        )
        {
            _actions = actions;
            _executor = executor;
            _state = state;
            _parser = parser;
            _logger = logger;
        }

        public abstract Task Handle(T obj, CancellationToken cancellationToken);

        private ParsedCommand? ParseText(string mText)
        {
            ParsedCommand? command = _parser.ParseText(mText);
            return command;
        }

        private async Task ParseAndHandleCommand(ChatId chatId, string mText, CancellationToken cancellationToken)
        {
            ParsedCommand? command = ParseText(mText);
            if (command == null) return;

            IBotAction? action = _actions.GetAction(command);
            if (action == null) return;

            ActionArgs args = command.Args;
            await _executor.Exec(chatId, action.Run, args, cancellationToken);
        }

        protected async Task HandleWithText(ChatId chatId, string mText, CancellationToken cancellationToken)
        {
            string? commandStr = await _state.GetCommandString(chatId);

            if (string.IsNullOrEmpty(commandStr))
                await ParseAndHandleCommand(chatId, mText, cancellationToken);
            
            else
            {
                string newCommandStr = $"{commandStr} {mText}";
                await ParseAndHandleCommand(chatId, newCommandStr, cancellationToken);

                await _state.ClearCommandString(chatId);
            }
        }
    }
}
