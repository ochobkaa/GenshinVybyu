using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.InputActions;

namespace GenshinVybyu.Actions.InputChains
{
    public class ProbInputChain : InputChainBase
    {
        public override string Name => "probchain";

        protected override void SetActions()
        {
            AddInputAction<FiftyFiftyInput>();
            AddInputAction<ConstsInput>();
            AddInputAction<RefinesInput>();
            AddInputAction<PrimoInput>();
            AddInputAction<RollsInput>();

            SetDestination<ProbAction>();
        }

        protected override ActionArgs ProcessInputCache(IList<string> inputCache)
        {
            string fiftyFifty = GetArg<FiftyFiftyInput>(inputCache);
            string consts = GetArg<ConstsInput>(inputCache);
            string refines = GetArg<RefinesInput>(inputCache);
            string primoStr = GetArg<PrimoInput>(inputCache);
            string rollsStr = GetArg<RollsInput>(inputCache);

            var args = new ActionArgs()
            {
                Args = new List<string>() { fiftyFifty, consts, refines },
                KwArgs = new Dictionary<string, string>
                {
                    {"rolls", rollsStr },
                    {"primos", primoStr }
                }
            };
            return args;
        }
    }
}
