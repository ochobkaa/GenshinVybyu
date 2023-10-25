using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public abstract class ActionBase : IBotAction
    {
        protected abstract IEnumerable<IncorrectArg>? CheckArgs(ActionArgs args);

        protected abstract Task OnIncorrectArgs(
            ActionContext actionContext, 
            IEnumerable<IncorrectArg> incorrectArgs, 
            CancellationToken cancellationToken
        );

        protected abstract Task OnRun(ActionContext actionContext, CancellationToken cancellationToken);

        public async Task Run(ActionContext actionContext, CancellationToken cancellationToken)
        {
            ActionArgs args = actionContext.ActionArgs;
            IEnumerable<IncorrectArg>? incorrectArgs = CheckArgs(args);

            if (incorrectArgs != null)
                await OnIncorrectArgs(actionContext, incorrectArgs, cancellationToken);

            else
                await OnRun(actionContext, cancellationToken);
        }
    }
}
