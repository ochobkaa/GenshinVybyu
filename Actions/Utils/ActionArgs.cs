using GenshinVybyu.Actions.Interfaces;

namespace GenshinVybyu.Actions.Utils
{
    public class ActionArgs
    {
        public static ActionArgs Empty => new()
        {
            Args = new List<string>(),
            KwArgs = new Dictionary<string, string>()
        };

        public IList<string>? Args { get; init; }
        public IDictionary<string, string>? KwArgs { get; init; }
    }
}
