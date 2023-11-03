using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IMessageBuilder
    {
        public BuildedMessage? BuildMessage(
            string messageName,
            bool addSplash,
            IDictionary<string, string>? replaces,
            RollsData? rollsData
        );
    }
}
