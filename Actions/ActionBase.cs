using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public abstract class ActionBase : IBotAction
    {
        protected delegate bool ArgsCondition(ActionArgs args);
        protected delegate Task OnFalseArgsConditionAction(
            ActionContext actionContext,
            CancellationToken cancellationToken
        );

        private class ArgsChecker { 
            public ArgsCondition Condition { get; init; }
            public OnFalseArgsConditionAction OnFalseAction { get; init; }
        }

        private List<ArgsChecker> _argsCheckers = new();

        public ActionBase()
        {
            AddCheckers();
        }

        private async Task<bool> CheckArgs(ActionContext actionContext, CancellationToken cancellationToken)
        {
            if (_argsCheckers.Count == 0) return true;

            ActionArgs args = actionContext.ActionArgs;

            foreach (var checker in _argsCheckers)
            {
                bool match = checker.Condition(args);
                if (!match)
                {
                    await checker.OnFalseAction(actionContext, cancellationToken);
                    return false;
                }
            }

            return true;
        }

        protected void AddChecker(ArgsCondition condition, OnFalseArgsConditionAction onFalseCondition)
        {
            var checker = new ArgsChecker() 
            { 
                Condition = condition, 
                OnFalseAction = onFalseCondition 
            };

            _argsCheckers.Add(checker);
        }

        protected virtual void AddCheckers() { }

        protected abstract Task OnRun(ActionContext actionContext, CancellationToken cancellationToken);

        public async Task Run(ActionContext actionContext, CancellationToken cancellationToken)
        {
            bool argsAreCorrect = await CheckArgs(actionContext, cancellationToken);

            if (argsAreCorrect)
                await OnRun(actionContext, cancellationToken);
        }
    }
}
