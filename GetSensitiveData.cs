using GenshinVybyu.Types;
using GenshinVybyu.Exceptions;
using Newtonsoft.Json;

namespace GenshinVybyu
{
    public static class GetSensitiveData
    {
        public static SensitiveData LoadData()
        {
            using var fs = new StreamReader("sensitive.json");

            string sensitiveDataRaw = fs.ReadToEnd();
            var sensitiveData = JsonConvert.DeserializeObject<SensitiveData>(sensitiveDataRaw);

            if (sensitiveData == null) throw new VybyuBotException("Failed to deserialize sensitive.json");

            return sensitiveData;
        }
    }
}
