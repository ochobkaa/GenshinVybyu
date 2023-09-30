using GenshinVybyu.Actions.Interfaces;

namespace GenshinVybyu.Actions.Utils
{
    public class ActionArgs
    {
        public static ActionArgs Empty => new()
        {
            Args = new List<string>(),
            KWArgs = new Dictionary<string, string>()
        };

        public IList<string> Args { get; init; }
        public IDictionary<string, string> KWArgs { get; init; }
    }
}
