using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Polynome
    {
        [JsonProperty(Required = Required.Always)]
        public int Start { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int End { get; set; }

        [JsonProperty]
        public double[] Coef { get; set; }
    }
}
