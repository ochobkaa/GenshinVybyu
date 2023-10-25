using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SensitiveData
    {
        [JsonProperty(Required = Required.Always)]
        public string BotToken { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string SecretToken { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string UserHashSeed { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string RedisConnectionString { get; set; }
    }
}
