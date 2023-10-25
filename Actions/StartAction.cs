using GenshinVybyu.Actions.Attributes;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace GenshinVybyu.Actions
{
    [BotAction("start", "")]
    public class StartAction : ActionBase
    {
        protected override IEnumerable<IncorrectArg>? CheckArgs(ActionArgs args) => null;

        protected override Task OnIncorrectArgs(
            ActionContext actionContext,
            IEnumerable<IncorrectArg> incorrectArgs,
            CancellationToken cancellationToken
        ) => Task.CompletedTask;

        protected override async Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
        {
            var output = actionContext.Output;
            ChatId chatId = actionContext.ChatId;

            await output.Message(chatId, "start", cancellationToken);
        }
    }
}
