using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions;
using GenshinVybyu.Actions.InputChains;

namespace GenshinVybyu
{
    public static class ActionExtensions
    {
        public static void MapActions(this IEndpointRouteBuilder builder)
        {
            var actionsCollection = builder.ServiceProvider.GetService<IActionsCollection>();

            actionsCollection
                .Bind<StartAction>()
                .Bind<ProbAction>()
                .Bind<RollsAction>();
        }

        public static void MapInputChains(this IEndpointRouteBuilder builder)
        {
            var actionsCollection = builder.ServiceProvider.GetService<IActionsCollection>();

            actionsCollection
                .Bind<ProbInputChain>()
                .Bind<RollsInputChain>();
        }
    }
}
