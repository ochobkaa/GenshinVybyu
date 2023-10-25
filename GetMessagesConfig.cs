using GenshinVybyu.Types;
using GenshinVybyu.Exceptions;
using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    public static class GetMessagesConfig
    {
        public static MessagesConfig LoadMessages()
        {
            using var fs = new StreamReader("messages.json");

            string messagesConfigRaw = fs.ReadToEnd();
            var messagesConfig = JsonConvert.DeserializeObject<MessagesConfig>(messagesConfigRaw);

            if (messagesConfig == null) throw new VybyuBotException("Failed to deserialize messages.json");

            return messagesConfig;
        }
    }
}
