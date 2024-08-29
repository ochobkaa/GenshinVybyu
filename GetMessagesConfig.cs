using GenshinVybyu.Types;
using GenshinVybyu.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace GenshinVybyu.Types
{
    public static class GetMessagesConfig
    {
        public static MessagesConfig LoadMessages()
        {
            using var fs = new StreamReader("messages.json", Encoding.UTF8);

            string messagesConfigRaw = fs.ReadToEnd();
            var messagesConfig = JsonConvert.DeserializeObject<MessagesConfig>(messagesConfigRaw);

            if (messagesConfig == null) throw new VybyuBotException("Failed to deserialize messages.json");

            return messagesConfig;
        }
    }
}
