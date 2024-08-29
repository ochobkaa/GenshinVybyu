using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GenshinVybyu.Services
{
    public class LoadModel : ILoadModel
    {
        private readonly BotConfiguration _botConf;
        private readonly ILogger _logger;

        public LoadModel(IOptions<BotConfiguration> botConf, ILogger<LoadModel> logger)
        {
            _botConf = botConf.Value;
            _logger = logger;
        }

        public async Task<ModelData> Load(bool fiftyfifty, int charConsts, int weaponRefines)
        {
            _logger.LogDebug($"Loading model: {(fiftyfifty ? "50/50" : "100%")} C{charConsts} R{weaponRefines}");

            string modelsPath = _botConf.ModelDataPath;

            string format = _botConf.ModelDataFilenameFormat;
            string filename = GetFilename(format, fiftyfifty, charConsts, weaponRefines);
            
            string modelFilePath = $"{modelsPath}/{filename}";
            ModelData modelData = await LoadModelFile(modelFilePath);

            return modelData;
        }

        private async Task<ModelData> LoadModelFile(string modelFilePath)
        {
            using var fs = new StreamReader(modelFilePath);
            string rawModelData = await fs.ReadToEndAsync();

            _logger.LogDebug($"File \"{modelFilePath}\" loaded");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            var modelData = JsonConvert.DeserializeObject<ModelData>(rawModelData, settings);

            if (modelData == null) throw new VybyuBotException($"Failed to deserialize \"{modelFilePath}\"");

            return modelData;
        }

        private static string GetFilename(string format, bool fiftyfifty, int charConsts, int weaponRefines)
        {
            string filename = format;
            filename = filename.Replace("{fiftyfifty}", fiftyfifty ? "50" : "100");
            filename = filename.Replace("{cchar}", $"{charConsts}");
            filename = filename.Replace("{rweapon}", $"{weaponRefines}");
            filename = $"{filename}.json";

            return filename;
        }
    }
}
