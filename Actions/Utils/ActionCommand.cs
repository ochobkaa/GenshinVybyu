using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.Utils
{
    public class ActionCommand
    {
        public string Name { get; init; } = default!;
        public IEnumerable<string> Tokens { get; init; }
    }
}
