using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.InputChains
{
    public abstract class InputChainBase : IInputChain
    {
        public abstract string Name { get; }
        public virtual IEnumerable<string>? Tokens => null;

        private List<IInputChainAction> _chainActions = new();
        private IBotAction _destAction;

        private Dictionary<Type, int> _indexes = new();

        public InputChainBase()
        {
            SetActions();
        }

        private async Task StartChain(
            ActionContext actionContext, 
            CancellationToken cancellationToken
        )
        {
            ChatId chatId = actionContext.ChatId;
            IChatStateActions state = actionContext.State;

            await state.StartInputChain(chatId, Name);
        }

        private async Task RunChainAction(
            int step,
            ActionContext actionContext,
            CancellationToken cancellationToken
        )
        {
            IInputChainAction action = _chainActions[step];
            ChatId chatId = actionContext.ChatId;
            IChatStateActions state = actionContext.State;

            await action.Run(actionContext, cancellationToken);

            await state.NextChainAction(chatId);
        } 

        private async Task FinishChain(
            ActionContext actionContext, 
            CancellationToken cancellationToken
        )
        {
            ChatId chatId = actionContext.ChatId;
            IChatStateActions state = actionContext.State;

            IList<string> inputCache = await state.FinishInputChain(chatId);
            ActionArgs args = ProcessInputCache(inputCache);

            ActionContext newActionContext = new()
            {
                ChatId = actionContext.ChatId,
                ActionArgs = args,
                Configuration = actionContext.Configuration,
                State = actionContext.State,
                Output = actionContext.Output,
                ModelCalc = actionContext.ModelCalc,
                Logger = actionContext.Logger
            };

            await _destAction.Run(newActionContext, cancellationToken);
        }

        protected void AddInputAction<TInputChainAction>()
            where TInputChainAction : IInputChainAction, new()
        {
            var action = new TInputChainAction();
            _chainActions.Add(action);
            _indexes.Add(typeof(TInputChainAction), _chainActions.Count - 1);
        }

        protected void SetDestination<TDestAction>() 
            where TDestAction : IBotAction, new()
        {
            var action = new TDestAction();
            _destAction = action;
        }

        protected string GetArg<TInputChainAction>(IList<string> inputCache)
            where TInputChainAction : IInputChainAction, new()
        {
            int actionIndex = _indexes[typeof(TInputChainAction)];

            string arg = inputCache[actionIndex];
            return arg;
        }

        protected abstract ActionArgs ProcessInputCache(IList<string> inputCache);

        protected abstract void SetActions();

        public async Task Run(ActionContext actionContext, CancellationToken cancellationToken)
        {
            ActionArgs args = actionContext.ActionArgs;
            ILogger logger = actionContext.Logger;

            if (args.Args == null || args.Args.Count == 0)
            {
                logger.LogDebug($"Starting input chain {Name}...");

                await StartChain(actionContext, cancellationToken);

                await RunChainAction(0, actionContext, cancellationToken);

                return;
            }

            if (int.TryParse(args.Args[0], out int step))
                if (step < _chainActions.Count)
                {
                    logger.LogDebug($"Performing input chain {Name}, step {step}...");

                    await RunChainAction(step, actionContext, cancellationToken);
                }

                else
                {
                    logger.LogDebug($"Finishing input chain {Name}...");

                    await FinishChain(actionContext, cancellationToken);
                }
        }
    }
}
