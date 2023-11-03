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

        public IActionsCollection Bind<TAction>()
            where TAction : IBotAction, new()
        {
            TAction action = new();

            ActionCommand actionCommand = new()
            {
                Name = action.Name,
                Tokens = action.Tokens ?? new List<string>()
            };

            _actionBinds.Add(actionCommand, action);
            return this;
        }

        public IBotAction? GetAction(ParsedCommand command)
        {
            string cmdToken = command.Token;
            IBotAction action = _actionBinds.FirstOrDefault(
                a => a.Key.Name == cmdToken 
                || a.Key.Tokens.Any(t => t == cmdToken)
            ).Value;

            return action;
        }
    }
}
