using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;

namespace GenshinVybyu.Services
{
    public class ModelsCollection : IModelsCollection
    {
        private readonly IServiceProvider _provider;

        private readonly Dictionary<string, ModelData> _models = new();

        public ModelsCollection(IServiceProvider provider)
        {
            _provider = provider;
        }

        private string GetModelKey(bool fiftyfifty, int consts, int refines)
            => $"{(fiftyfifty ? 50 : 100)}_c{consts}_r{refines}";

        public async Task<ModelData> GetModelData(bool fiftyfifty, int consts, int refines)
        {
            string key = GetModelKey(fiftyfifty, consts, refines);

            _models.TryGetValue(key, out var model);
            if (model != null)
                return model;

            var loader = _provider.GetService<ILoadModel>();
            ModelData loadedModel = await loader.Load(fiftyfifty, consts, refines);
            _models.Add(key, loadedModel);

            return loadedModel;
        }
    }
}
