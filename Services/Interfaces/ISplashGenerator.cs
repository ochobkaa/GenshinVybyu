using GenshinVybyu.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface ISplashGenerator : IService
    {
        public Splash? GetSplash();
    }
}
