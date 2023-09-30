using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Actions.Interfaces
{
    public interface IBotAction
    {
        public Task Run(ActionContext actionContext, CancellationToken cancellationToken);
    }
}
