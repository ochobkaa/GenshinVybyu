using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.Checkers
{
    public class RollsPrimoChecker : IArgsChecker
    {
        private bool CheckRolls(ActionArgs args)
            => args.KwArgs.TryGetValue("rolls", out _);

        private bool CheckPrimo(ActionArgs args)
            => args.KwArgs.TryGetValue("primo", out _);

        public bool Check(ActionArgs args)
            => CheckRolls(args) || CheckPrimo(args);

        public async Task OnFalse(ActionContext context, CancellationToken cancellationToken)
        {
            ChatId chatId = context.ChatId;
            IBotOutput output = context.Output;

            await output.Message(
                chatId,
                "onFalseRollsPrimo",
                cancellationToken,
                addSplash: true
            );
        }
    }
}
