using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Splash
    {
        [JsonProperty(Required = Required.Always)]
        public string Label { get; set; }

        [JsonProperty]
        public string? Link { get; set; }
    }
}
