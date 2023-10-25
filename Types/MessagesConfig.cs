using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MessagesConfig
    {
        [JsonProperty(Required = Required.Always)]
        public IList<Splash> Splashes { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IList<BotMessage> Messages { get; set; }
    }
}
