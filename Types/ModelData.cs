using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ModelData
    {
        [JsonProperty(Required = Required.Always)]
        public bool FiftyFifty { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Consts { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Refines { get; set; }

        [JsonProperty]
        public Polynome[] Polynomes { get; set; }
    }
}
