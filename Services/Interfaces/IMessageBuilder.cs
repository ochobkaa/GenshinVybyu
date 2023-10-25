using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IMessageBuilder
    {
        public BuildedMessage? BuildMessage(string messageName, bool addSplash=false);
    }
}
