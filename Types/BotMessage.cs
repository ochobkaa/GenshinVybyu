using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BotMessage
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty]
        public IEnumerable<BotKeyboardButton>? Keyboard { get; set; }
    }
}
