using GenshinVybyu.Actions.Attributes;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Actions
{
    [BotAction("prob", "probability вер вероятность")]
    public class ProbAction : ActionBase
    {
        protected override IEnumerable<IncorrectArg>? CheckArgs(ActionArgs args)
        {
            var incorrectArgs = new List<IncorrectArg>();

            var flatArgs = args.Args;
            var kwArgs = args.KwArgs;

            bool containsCharConsts = flatArgs.Any(a => Regex.IsMatch(a, "c[0-6]"))
                || kwArgs.ContainsKey("c");

            bool containsWeaponRefines = flatArgs.Any(a => Regex.IsMatch(a, "r[1-5]"))
                || kwArgs.ContainsKey("r");

            bool containsProb = flatArgs.Any(a => int.TryParse(a, out int prob) && prob > 0 && prob < 100)
                || kwArgs.ContainsKey("prob")
                || kwArgs.ContainsKey("вер")
                || kwArgs.ContainsKey("вероятность");

            if ((containsCharConsts || containsWeaponRefines) && containsProb)
                return null;

            if (!containsProb)
                incorrectArgs.Add(new() { Name = "Prob", Message = "Не указана вероятность!" });

            if (!containsCharConsts)
                incorrectArgs.Add(new() { Name = "Consts" });

            if (!containsWeaponRefines)
                incorrectArgs.Add(new() { Name = "Refines" });

            return incorrectArgs;
        }

        protected override Task OnRun(ActionContext actionContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task OnIncorrectArgs(ActionContext actionContext, IEnumerable<IncorrectArg> incorrectArgs, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
