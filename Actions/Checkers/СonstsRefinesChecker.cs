using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.Checkers
{
    public class СonstsRefinesChecker : IArgsChecker
    {
        private bool CheckRefines(ActionArgs args)
            => args.Args?.Any(s => Regex.IsMatch(s, "r[1-5]")) ?? false;

        private bool CheckConsts(ActionArgs args)
            => args.Args?.Any(s => Regex.IsMatch(s, "c[0-6]")) ?? false;

        public bool Check(ActionArgs args)
            => CheckConsts(args) || CheckRefines(args);

        public async Task OnFalse(ActionContext context, CancellationToken cancellationToken)
        {
            ChatId chatId = context.ChatId;
            IBotOutput output = context.Output;

            await output.Message(
                chatId,
                "onFalseConstsRefines",
                cancellationToken,
                addSplash: true
            );
        }
    }
}
