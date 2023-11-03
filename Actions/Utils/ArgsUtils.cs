using System.Text.RegularExpressions;

namespace GenshinVybyu.Actions.Utils
{
    public static class ArgsUtils
    {
        public static int IntFromArgs(int index, ActionArgs args)
        {
            string valStr = args.Args[index];
            int.TryParse(valStr, out int val);

            return val;
        }

        public static double DoubleFromArgs(int index, ActionArgs args)
        {
            string valStr = args.Args[index];
            double.TryParse(valStr, out double val);

            return val;
        }

        public static int IntFromKwArgs(string key, ActionArgs args)
        {
            args.KwArgs.TryGetValue(key, out string rollsStr);
            int.TryParse(rollsStr, out int val);

            return val;
        }

        public static double DoubleFromKwArgs(string key, ActionArgs args)
        {
            args.KwArgs.TryGetValue(key, out string rollsStr);
            double.TryParse(rollsStr, out double val);

            return val;
        }

        public static bool FiftyFiftyFromArgs(ActionArgs args)
            => args.Args?.Contains("ff") ?? false;

        public static int ConstsFromArgs(ActionArgs args)
        {
            string? constsStr = args.Args?.FirstOrDefault(
                s => Regex.IsMatch(s, "c[0-6]")
            );

            int consts = constsStr switch
            {
                "c0" => 0,
                "c1" => 1,
                "c2" => 2,
                "c3" => 3,
                "c4" => 4,
                "c5" => 5,
                "c6" => 6,
                _ => -1
            };

            return consts;
        }

        public static int RefinesFromArgs(ActionArgs args)
        {
            string? refinesStr = args.Args?.FirstOrDefault(
                s => Regex.IsMatch(s, "r[1-5]")
            );

            int refines = refinesStr switch
            {
                "r1" => 1,
                "r2" => 2,
                "r3" => 3,
                "r4" => 4,
                "r5" => 5,
                _ => 0
            };

            return refines;
        }
    }
}
