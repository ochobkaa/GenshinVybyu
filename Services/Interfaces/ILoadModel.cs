using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface ILoadModel : IService
    {
        public Task<ModelData> Load(bool fiftyfifty, int charConsts, int weaponRefines);
    }
}
