using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IModelsCollection : IService
    {
        public Task<ModelData> GetModelData(bool fiftyfifty, int consts, int refines);
    }
}
