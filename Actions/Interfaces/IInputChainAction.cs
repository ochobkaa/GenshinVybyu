using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Actions.Interfaces
{
    public interface IInputChainAction
    {
        public Task Run(ActionContext actionContext, CancellationToken cancellationToken);
    }
}
