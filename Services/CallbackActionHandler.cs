using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class CallbackActionsHandler : ActionsHandlerBase<CallbackQuery>
    {
        public CallbackActionsHandler(
            IActionsCollection actions,
            IActionExecutor executor,
            IChatStateActions state,
            ICommandParser parser,
            IOptions<BotConfiguration> conf,
            ILogger<CallbackActionsHandler> logger
        ) : base(actions, executor, state, parser, conf, logger) { }

        public override async Task Handle(CallbackQuery cbQuery, CancellationToken cancellationToken)
        {
            string? mText = cbQuery.Data;
            if (string.IsNullOrEmpty(mText)) return;

            ChatId chatId = cbQuery.From.Id;

            await HandleWithText(chatId, mText, cancellationToken);
        }
    }
}
