using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public abstract class ActionBase : IBotAction
    {
        protected abstract IEnumerable<IncorrectArg>? CheckArgs(ActionArgs args);

        protected abstract Task OnRun(ActionContext actionContext, CancellationToken cancellationToken);

        public async Task Run(ActionContext actionContext, CancellationToken cancellationToken)
        {
            ITelegramBotClient client = actionContext.Client;
            ChatId chatId = actionContext.ChatId;

            ActionArgs args = actionContext.ActionArgs;
            IEnumerable<IncorrectArg>? incorrectArgs = CheckArgs(args);

            if (incorrectArgs != null)
                await SendIncorrectArgMessage(client, chatId, incorrectArgs, cancellationToken);

            else
                await OnRun(actionContext, cancellationToken);
        }

        private async Task SendIncorrectArgMessage(
            ITelegramBotClient client, 
            ChatId chatId, 
            IEnumerable<IncorrectArg> incorrectArgs,
            CancellationToken cancellationToken
        )
        {
            string message = "Неверно введены аргументы:\n";
            foreach (var arg in incorrectArgs)
                message = $"{message}{arg.Message}\n";

            await client.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);
        }
    }
}
