using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Services
{
    public class ActionsCollection : IActionsCollection
    {

        private readonly Dictionary<string, IBotAction> _actionBinds = new();

        public IActionsCollection Bind<TAction>()
            where TAction : IBotAction, new()
        {
            TAction action = new();

            _actionBinds[action.Name] = action;

            if (action.Tokens != null)
                foreach (string token in action.Tokens)
                    _actionBinds[token] = action;

            return this;
        }

        public IBotAction? GetAction(ParsedCommand command)
        {
            string cmdToken = command.Token;
            if (_actionBinds.ContainsKey(cmdToken))
            {
                IBotAction action = _actionBinds[cmdToken];
                return action;
            }
            else
            {
                return null;
            }
        }
    }
}
