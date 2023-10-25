namespace GenshinVybyu.Actions.Utils
{
    public class ParsedCommand
    {
        public string Token { get; init; }
        public ActionArgs Args { get; init; }
        public string CommandPrefix { get; init; }
        public string KeyAttrValuePrefix { get; init; }

        public override string ToString()
        {
            string argsStr = "";
            string kwArgsStr = "";
            if (Args != null)
            {
                if (Args.Args != null)
                    argsStr = string.Join(" ", Args.Args);

                if (Args.KwArgs != null) {
                    IEnumerable<string> kwArgsStrs = Args.KwArgs.Select(
                        kw => $"{kw.Key}{KeyAttrValuePrefix}{kw.Value}"
                    );
                    kwArgsStr = string.Join(" ", kwArgsStrs);
                }
            }

            string str = $"{CommandPrefix}{Token} {argsStr} {kwArgsStr}";
            return str;
        }
    }
}
