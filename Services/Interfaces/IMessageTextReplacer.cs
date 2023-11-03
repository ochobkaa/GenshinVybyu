using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IMessageTextReplacer
    {
        public string ReplaceText(string message, IDictionary<string, string> replaces);
        public string InsertRollsData(string message, RollsData rollsData);
    }
}
