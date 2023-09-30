using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services
{
    public class MessageActionsHandler : ActionsHandlerBase<Message>
    {
        public MessageActionsHandler(
            IActionsCollection actions,
            IActionExecutor executor,
            ICommandParser parser,
            ILogger logger
        ) : base(actions, executor, parser, logger) { }

        public override async Task Handle(Message message, CancellationToken cancellationToken)
        {
            string? mText = message.Text;
            if (string.IsNullOrEmpty(mText)) return;

            ChatId chatId = message.Chat.Id;

            await HandleWithText(chatId, mText, cancellationToken);
        }
    }
}
