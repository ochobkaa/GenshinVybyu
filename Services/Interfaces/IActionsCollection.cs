using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IActionsCollection : IService
    {
        public IActionsCollection Bind<TAction>()
            where TAction : IBotAction, new();
        public IBotAction? GetAction(ParsedCommand command);
    }
}
