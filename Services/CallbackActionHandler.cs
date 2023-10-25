using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services
{
    public class CallbackActionsHandler : ActionsHandlerBase<CallbackQuery>
    {
        public CallbackActionsHandler(
            IActionsCollection actions,
            IActionExecutor executor,
            ICommandParser parser,
            ILogger<CallbackActionsHandler> logger
        ) : base(actions, executor, parser, logger) { }

        public override async Task Handle(CallbackQuery cbQuery, CancellationToken cancellationToken)
        {
            string? mText = cbQuery.Data;
            if (string.IsNullOrEmpty(mText)) return;

            ChatId chatId = cbQuery.ChatInstance;

            await HandleWithText(chatId, mText, cancellationToken);
        }
    }
}
