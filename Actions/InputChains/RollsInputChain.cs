using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.InputActions;

namespace GenshinVybyu.Actions.InputChains
{
    public class RollsInputChain : InputChainBase
    {
        public override string Name => "rollschain";

        protected override void SetActions()
        {
            AddInputAction<FiftyFiftyInput>();
            AddInputAction<ConstsInput>();
            AddInputAction<RefinesInput>();
            AddInputAction<ProbInput>();

            SetDestination<RollsAction>();
        }

        protected override ActionArgs ProcessInputCache(IList<string> inputCache)
        {
            string fiftyFifty = GetArg<FiftyFiftyInput>(inputCache);
            string consts = GetArg<ConstsInput>(inputCache);
            string refines = GetArg<RefinesInput>(inputCache);
            string probStr = GetArg<ProbInput>(inputCache);

            var args = new ActionArgs()
            {
                Args = new List<string>() { fiftyFifty, consts, refines },
                KwArgs = new Dictionary<string, string>
                {
                    {"prob", probStr },
                }
            };
            return args;
        }
    }
}
