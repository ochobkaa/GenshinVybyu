using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Attributes;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Actions.Interfaces;
using System.Reflection;

namespace GenshinVybyu
{
    public static class ActionExtensions
    {
        public static void MapActions<T>(this IEndpointRouteBuilder builder)
            where T : IActionsCollection
        {
            var actionsCollection = builder.ServiceProvider.GetService<IActionsCollection>();

            var actions = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterface("IBotAction") != null)
                .Where(t => !t.IsAbstract);
            if (actions == null) return;

            foreach (var action in actions)
            {
                ActionCommand command = action.GetCustomAttribute<BotActionAttribute>().Command;

                if (command != null)
                {
                    var actionObj = Activator.CreateInstance(action) as IBotAction;
                    actionsCollection.Bind(command, actionObj);
                }
            }
        }
    }
}
