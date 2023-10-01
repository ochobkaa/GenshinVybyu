using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

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
            string commandToken = rawTextCommand[..commandEndPos];
            return commandToken;
        }

        private static List<string> GetArgs(string argValPrefix, string[] argStrs)
        {
            List<string> args = argStrs.Where(s => !s.Contains(argValPrefix)).ToList();

            return args;
        }

        private static Dictionary<string, string> GetKWArgs(string argValPrefix, string[] argStrs)
        {
            Dictionary<string, string> kwArgs = (Dictionary<string, string>)
                argStrs.Where(s => s.Contains(argValPrefix))
                    .Select(s =>
                    {
                        string[] spl = s.Split(argValPrefix);
                        var kwPair = new KeyValuePair<string, string>(spl[0], spl[1]);
                        return kwPair;
                    });

            return kwArgs;
        }

        private ActionArgs? GetActionArgs(string rawTextCommand)
        {
            int argsStartPos = rawTextCommand.IndexOf(" ") + 1;
            string rawArgs = rawTextCommand[argsStartPos..];

            string[] argStrs = Regex.Split(rawArgs, "(?=(?:[^\"]* \"[^\"]*\")*[^\"]*$)");
            argStrs = argStrs.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            string argValPrefix = _conf.KeyAttrValuePrefix;
            List<string> args = GetArgs(argValPrefix, argStrs);
            Dictionary<string, string> kwArgs = GetKWArgs(argValPrefix, argStrs);
            ActionArgs actionArgs = new()
            {
                Args = args,
                KwArgs = kwArgs
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
                CommandToken = commandToken,
                Args = args ?? ActionArgs.Empty
            };

            return parsedCommand;
        }
    }
}
