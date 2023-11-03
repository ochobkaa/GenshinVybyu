using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RollsDataFormat
    {
        [JsonProperty(Required = Required.Always)]
        public string ConstsOnly { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string RefinesOnly { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string ConstsAndRefines { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string FiftyFifty { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string NoFiftyFifty { get; set; }
    }
}
