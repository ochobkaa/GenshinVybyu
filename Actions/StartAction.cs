using GenshinVybyu.Actions.Attributes;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    [BotAction("start", "")]
    public class StartAction : ActionBase
    {
        protected override async Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
        {
            var output = actionContext.Output;
            ChatId chatId = actionContext.ChatId;

            await output.Message(chatId, "start", cancellationToken);
        }
    }
}
