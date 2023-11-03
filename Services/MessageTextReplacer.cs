using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;

namespace GenshinVybyu.Services
{
    public class MessageTextReplacer : IMessageTextReplacer
    {
        private readonly IRollsDataFormatter _rdFormatter;

        public MessageTextReplacer(IRollsDataFormatter rdFormatter)
        {
            _rdFormatter = rdFormatter;
        }

        public string ReplaceText(string message, IDictionary<string, string> replaces)
        {
            string newMessage = message;
            foreach (var replace in replaces)
            {
                newMessage = newMessage.Replace(replace.Key, replace.Value);
            }

            return newMessage;
        }

        public string InsertRollsData(string message, RollsData rollsData)
        {
            string rollsDataStr = _rdFormatter.GetFomatted(rollsData);
            string newMessage = ReplaceText(
                message, new Dictionary<string, string>() { { "{rollsData}", rollsDataStr } }
            );

            return newMessage;
        }
    }
}
