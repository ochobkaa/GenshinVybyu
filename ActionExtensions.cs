using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions;

namespace GenshinVybyu
{
    public static class ActionExtensions
    {
        public static void MapActions(this IEndpointRouteBuilder builder)
        {
            var actionsCollection = builder.ServiceProvider.GetService<IActionsCollection>();

            actionsCollection
                .Bind<StartAction>()
                .Bind<ProbAction>();
        }
    }
}
