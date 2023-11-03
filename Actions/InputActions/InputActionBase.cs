using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.InputActions
{
    public abstract class InputActionBase : IInputChainAction
    {
        protected abstract string MessageName { get; }

        protected virtual Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public async Task Run(ActionContext actionContext, CancellationToken cancellationToken)
        {
            ChatId chatId = actionContext.ChatId;
            IBotOutput output = actionContext.Output;

            await output.Message(
                chatId,
                MessageName,
                cancellationToken
            );

            await OnRun(actionContext, cancellationToken);
        }
    }
}
