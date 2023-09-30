using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Services.Interfaces
{
    public interface ICommandParser : IService
    {
        public ParsedCommand? ParseText(string text);
    }
}
