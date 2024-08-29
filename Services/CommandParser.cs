using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GenshinVybyu.Services
{
    public class CommandParser : ICommandParser
    {
        private readonly BotConfiguration _conf;

        public CommandParser(IOptions<BotConfiguration> options)
        {
            _conf = options.Value;
        }

        private string GetCommandToken(string rawTextCommand)
        {
            int commandEndPos = rawTextCommand.IndexOf(" ");
            if (commandEndPos > -1)
            {
                string commandToken = rawTextCommand[..commandEndPos];
                return commandToken;
            }
            else
            {
                return rawTextCommand;
            }
        }

        private static List<string> GetArgs(string argValPrefix, string[] argStrs)
        {
            List<string> args = argStrs.Where(s => !s.Contains(argValPrefix)).ToList();

            return args;
        }

        private static Dictionary<string, string> GetKWArgs(string argValPrefix, string[] argStrs)
        {
            Dictionary<string, string> kwArgs = new();
            foreach (string argStr in argStrs)
            {
                if (argStr.Contains(argValPrefix))
                {
                    string[] split = argStr.Split(argValPrefix);

                    string argName = split[0];
                    string argVal = split[1];
                    kwArgs[argName] = argVal;
                }
            }

            return kwArgs;
        }

        private ActionArgs? GetActionArgs(string rawTextCommand)
        {
            if (string.IsNullOrEmpty(rawTextCommand))
                return null;

            int argsStartPos = rawTextCommand.IndexOf(" ");
            if (argsStartPos == -1)
                return null;

            string rawArgs = rawTextCommand[(argsStartPos + 1)..];

            string[] argStrs = rawArgs.Split(" ");

            argStrs = argStrs.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            if (argStrs.Length == 0)
                return null;

            string argValPrefix = _conf.KeyAttrValuePrefix;
            List<string> args = GetArgs(argValPrefix, argStrs);
            Dictionary<string, string> kwArgs = GetKWArgs(argValPrefix, argStrs);
            ActionArgs actionArgs = new()
            {
                Args = args,
                KwArgs = kwArgs,
            };

            return actionArgs;
        }

        public ParsedCommand? ParseText(string text)
        {
            string cmdPrefix = _conf.CommandPrefix;
            bool isCommand = text.StartsWith(cmdPrefix);
            if (!isCommand) return null;

            string rawCommand = text[1..];

            string commandToken = GetCommandToken(rawCommand);
            ActionArgs? args = GetActionArgs(rawCommand);

            var parsedCommand = new ParsedCommand()
            {
                Token = commandToken,
                Args = args ?? ActionArgs.Empty,
                CommandPrefix = _conf.CommandPrefix,
                KeyAttrValuePrefix = _conf.KeyAttrValuePrefix
            };

            return parsedCommand;
        }
    }
}
