using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IActionsHandler<T> : IService
    {
        public Task Handle(T obj, CancellationToken cancellationToken);
    }
}
