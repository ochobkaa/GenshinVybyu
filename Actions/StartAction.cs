using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public class StartAction : ActionBase
    {
        public override string Name => "start";

        protected override async Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
        {
            IBotOutput output = actionContext.Output;
            ChatId chatId = actionContext.ChatId;

            await output.Message(chatId, "start", cancellationToken);
        }
    }
}
