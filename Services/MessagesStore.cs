using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class MessagesStore : IMessagesStore
    {
        private readonly Dictionary<string, BotMessage> _messages = new();

        public MessagesStore(IOptions<MessagesConfig> msgConfig)
        {
            _AddMessages(msgConfig.Value);
        }

        private void _AddMessages(MessagesConfig msgConfig)
        {
            foreach (BotMessage msg in msgConfig.Messages)
                _messages[msg.Name] = msg;
        }

        public BotMessage MessageByName(string name)
        {
            if (_messages.TryGetValue(name, out var msg))
                return msg;

            else
            {
                var emptyMsg = new BotMessage() { Name = name, Text = name };
                return emptyMsg;
            }
        }
    }
}
