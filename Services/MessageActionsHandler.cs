using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class MessageActionsHandler : ActionsHandlerBase<Message>
    {
        public MessageActionsHandler(
            IActionsCollection actions,
            IActionExecutor executor,
            IChatStateActions state,
            ICommandParser parser,
            IOptions<BotConfiguration> conf,
            ILogger<MessageActionsHandler> logger
        ) : base(actions, executor, state, parser, conf, logger) { }

        public override async Task Handle(Message message, CancellationToken cancellationToken)
        {
            string? mText = message.Text;
            if (string.IsNullOrEmpty(mText)) return;

            ChatId chatId = message.Chat.Id;

            await HandleWithText(chatId, mText, cancellationToken);
        }
    }
}
