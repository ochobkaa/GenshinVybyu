using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public record class ChatState
    {
        [JsonProperty]
        public string? Command { get; set; }

        [JsonProperty]
        public string? SuperUser { get; set; }
    }
}
