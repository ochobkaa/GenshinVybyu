using GenshinVybyu.Services;
using GenshinVybyu.Services.Interfaces;
using Telegram.Bot.Types;

namespace GenshinVybyu
{
    public static class AddServices
    {
        public static IServiceCollection AddModelCalc(this IServiceCollection services)
        {
            services
                .AddTransient<ILoadModel, LoadModel>()
                .AddSingleton<IModelsCollection, ModelsCollection>()
                .AddScoped<IModelCalc, ModelCalc>();

            return services;
        }

        public static IServiceCollection AddState(this IServiceCollection services)
        {
            services
                .AddTransient<IChatStateManager, ChatStateManager>()
                .AddScoped<IChatStateActions, ChatStateActions>();

            return services;
        }

        public static IServiceCollection AddHandlingPipeline(this IServiceCollection services)
        {
            services
                .AddSingleton<IActionsCollection, ActionsCollection>()
                .AddScoped<IActionExecutor, ActionExecutor>()
                .AddScoped<ICommandParser, CommandParser>()
                .AddScoped<IActionsHandler<Message>, MessageActionsHandler>()
                .AddScoped<IActionsHandler<CallbackQuery>, CallbackActionsHandler>()
                .AddScoped<IBotHandler, BotHandler>();

            return services;
        }

        public static IServiceCollection AddBotOutput(this IServiceCollection services)
        {
            services
                .AddScoped<ISplashGenerator, SplashGenerator>()
                .AddScoped<IRollsDataFormatter, RollsDataFormatter>()
                .AddScoped<IMessageTextReplacer, MessageTextReplacer>()
                .AddScoped<IMessagesStore, MessagesStore>()
                .AddScoped<IMessageBuilder, MessageBuilder>()
                .AddScoped<IBotOutput, BotOutput>();

            return services;
        }

        public static IServiceCollection AddUtility(this IServiceCollection services)
        {
            services
                .AddScoped<IHashChat, HashChat>()
                .AddScoped<IRequestRepeater, RequestRepeater>();

            return services;
        }
    }
}
