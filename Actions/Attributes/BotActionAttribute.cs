using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Actions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BotActionAttribute : Attribute
    {
        public ActionCommand Command { get; init; }

        public BotActionAttribute(string name, string tokens)
        {
            var splittedTokens = tokens.Split(" ");

            Command = new()
            {
                Name = name,
                Tokens = splittedTokens
            };
        }
    }
}
