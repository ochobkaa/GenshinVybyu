using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class RollsDataFormatter : IRollsDataFormatter
    {
        private readonly RollsDataFormat _format;

        public RollsDataFormatter(IOptions<MessagesConfig> mConf)
        {
            _format = mConf.Value.RollsDataFormat;
        }

        private string GetFiftyFiftyStr(bool fiftyFifty)
            => fiftyFifty ? _format.FiftyFifty : _format.NoFiftyFifty;

        private string GetConstsStr(bool fiftyFifty, int consts)
        {
            string format = _format.ConstsOnly;

            string fiftyFiftyStr = GetFiftyFiftyStr(fiftyFifty);
            string constsStr = format
                .Replace("{consts}", $"{consts}")
                .Replace("{fiftyFifty}", fiftyFiftyStr);

            return constsStr;
        }

        private string GetRefinesStr(int refines)
        {
            string format = _format.RefinesOnly;

            string refinesStr = format.Replace("{refines}", $"{refines}");

            return refinesStr;
        }

        private string GetConstsRefinesStr(bool fiftyFifty, int consts, int refines)
        {
            string format = _format.ConstsAndRefines;

            string fiftyFiftyStr = GetFiftyFiftyStr(fiftyFifty);
            string constsRefinesStr = format
                .Replace("{consts}", $"{consts}")
                .Replace("{refines}", $"{refines}")
                .Replace("{fiftyFifty}", fiftyFiftyStr);

            return constsRefinesStr;
        }

        public string GetFomatted(RollsData rollsData)
        {
            bool fiftyFifty = rollsData.FiftyFifty;
            int consts = rollsData.Consts;
            int refines = rollsData.Refines;

            bool rollChar = consts >= 0 && consts <= 6;
            bool rollWeapon = refines >= 1 && refines <= 5;

            string formatted = "";
            if (rollChar && rollWeapon)
                formatted = GetConstsRefinesStr(fiftyFifty, consts, refines);

            else if (rollChar)
                formatted = GetConstsStr(fiftyFifty, consts);

            else if (rollWeapon)
                formatted = GetRefinesStr(refines);

            return formatted;
        }
    }
}
