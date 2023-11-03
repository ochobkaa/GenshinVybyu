using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public record class ChatState
    {
        [JsonProperty]
        public InputChainState? InputChain { get; set; }

        [JsonProperty]
        public bool? SuperUser { get; set; }
    }
}
