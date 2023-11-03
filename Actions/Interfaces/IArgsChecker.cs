using GenshinVybyu.Actions.Utils;

namespace GenshinVybyu.Actions.Interfaces
{
    public interface IArgsChecker
    {
        public bool Check(ActionArgs args);
        public Task OnFalse(ActionContext context, CancellationToken cancellationToken);
    }
}
