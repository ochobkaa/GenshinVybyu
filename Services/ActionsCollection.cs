using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class ActionsCollection : IActionsCollection
    {
        private readonly Dictionary<ActionCommand, IBotAction> _actionBinds = new();

        public void Bind(ActionCommand actionCommand, IBotAction action)
        {
            _actionBinds.Add(actionCommand, action);
        }

        public IBotAction? GetAction(ParsedCommand command)
        {
            string cmdToken = command.CommandToken;
            IBotAction action = _actionBinds.FirstOrDefault(
                a => a.Key.Tokens.Any(t => t == cmdToken)
            ).Value;

            return action;
        }
    }
}
