using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IActionsCollection : IService
    {
        public void Bind(ActionCommand actionCommand, IBotAction action);
        public IBotAction? GetAction(ParsedCommand command);
    }
}
