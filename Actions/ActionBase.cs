using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public abstract class ActionBase : IBotAction
    {
        private List<IArgsChecker> _argsCheckers = new();

        public abstract string Name { get; }
        public virtual IEnumerable<string>? Tokens => null;

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
                bool match = checker.Check(args);
                if (!match)
                {
                    await checker.OnFalse(actionContext, cancellationToken);
                    return false;
                }
            }

            return true;
        }

        protected void AddChecker<TChecker>()
            where TChecker : IArgsChecker, new()
        {
            var checker = new TChecker();

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
