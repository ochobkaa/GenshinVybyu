using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.Checkers;
using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public class RollsAction : ActionBase
    {
        public override string Name => "rolls";
        public override IEnumerable<string> Tokens
            => new List<string>() { "r" };

        protected override void AddCheckers()
        {
            AddChecker<СonstsRefinesChecker>();
            AddChecker<ProbChecker>();
        }

        private Dictionary<string, string> GetReplaces(int rolls, double prob)
            => new()
            {
                { "{rolls}", $"{rolls}" },
                { "{prob}", $"{prob}" }
            };

        protected override async Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
        {
            ChatId chatId = actionContext.ChatId;
            ActionArgs args = actionContext.ActionArgs;
            IModelCalc calc = actionContext.ModelCalc;
            IBotOutput output = actionContext.Output;

            bool fiftyFifty = ArgsUtils.FiftyFiftyFromArgs(args);
            int consts = ArgsUtils.ConstsFromArgs(args);
            int refines = ArgsUtils.RefinesFromArgs(args);

            double prob = ArgsUtils.DoubleFromKwArgs("prob", args);

            int rolls = await calc.GetRolls(prob, fiftyFifty, consts, refines);

            var rollsData = new RollsData()
            {
                FiftyFifty = fiftyFifty,
                Consts = consts,
                Refines = refines
            };
            Dictionary<string, string> replaces = GetReplaces(rolls, prob);

            await output.Message(
                chatId, "rolls",
                cancellationToken,
                addSplash: true,
                replaces: replaces,
                rollsData: rollsData
            );
        }
    }
}
