using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.Checkers
{
    public class ProbChecker : IArgsChecker
    {
        public bool Check(ActionArgs args)
            => args.KwArgs?.ContainsKey("prob") ?? false;

        public async Task OnFalse(ActionContext context, CancellationToken cancellationToken)
        {
            ChatId chatId = context.ChatId;
            IBotOutput output = context.Output;

            await output.Message(
                chatId,
                "onFalseProb",
                cancellationToken,
                addSplash: true
            );
        }
    }
}
