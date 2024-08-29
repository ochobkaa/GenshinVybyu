using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.Checkers;
using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions
{
    public class ProbAction : ActionBase
    {
        public override string Name => "probability";
        public override IEnumerable<string> Tokens 
            => new List<string>() { "pr", "prob" };

        protected override void AddCheckers()
        {
            AddChecker<СonstsRefinesChecker>();
            AddChecker<RollsPrimoChecker>();
        }

        private Dictionary<string, string> GetReplaces(int rolls, int primos, double prob100)
            => new() 
            {
                { "{rolls}", $"{rolls}" },
                { "{primos}", $"{primos}" },
                { "{prob}", $"{prob100:F2}" }
            };

        protected override async Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
        {
            ChatId chatId = actionContext.ChatId;
            ActionArgs args = actionContext.ActionArgs;
            IModelCalc calc = actionContext.ModelCalc;
            IBotOutput output = actionContext.Output;
            ILogger logger = actionContext.Logger;

            bool fiftyFifty = ArgsUtils.FiftyFiftyFromArgs(args);
            int consts = ArgsUtils.ConstsFromArgs(args);
            int refines = ArgsUtils.RefinesFromArgs(args);

            int rolls = ArgsUtils.IntFromKwArgs("rolls", args) ?? 0;
            int primos = ArgsUtils.IntFromKwArgs("primos", args) ?? 0;

            double prob = await calc.GetProbability(rolls, fiftyFifty, consts, refines, primos);
            double prob100 = prob * 100;

            var rollsData = new RollsData()
            {
                FiftyFifty = fiftyFifty,
                Consts = consts,
                Refines = refines
            };
            Dictionary<string, string> replaces = GetReplaces(rolls, primos, prob100);

            await output.Message(
                chatId, "probability", 
                cancellationToken,
                addSplash: true,
                replaces: replaces,
                rollsData: rollsData
            );
        }
    }
}
