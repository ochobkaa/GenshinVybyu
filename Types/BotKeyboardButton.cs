using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BotKeyboardButton
    {
        [JsonProperty(Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Label { get; set; }

        [JsonProperty]
        public string? Command { get; set; }

        [JsonProperty]
        public IEnumerable<string>? Args { get; set; }

        [JsonProperty]
        public IEnumerable<KwArg>? KwArgs { get; set; }

        [JsonProperty]
        public string? Message { get; set; }


    }
}
