using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Types;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class ActionExecutor : IActionExecutor
    {
        private readonly ITelegramBotClient _client;
        private readonly IModelCalc _calc;
        private readonly BotConfiguration _conf;
        private readonly ILogger _logger;

        public ActionExecutor(
            ITelegramBotClient client,
            IModelCalc calc,
            IOptions<BotConfiguration> options,
            ILogger logger
        )
        {
            _client = client;
            _calc = calc;
            _conf = options.Value;
            _logger = logger;
        }

        public async Task Exec(
            ChatId chatId,
            Func<ActionContext, CancellationToken, Task> actionCb, 
            ActionArgs args, 
            CancellationToken cancellationToken
        )
        {
            ActionContext context = CreateActionContext(chatId, args);
            await actionCb(context, cancellationToken);
        }

        private ActionContext CreateActionContext(ChatId chatId, ActionArgs args)
        {
            var context = new ActionContext()
            {
                Client = _client,
                ModelCalc = _calc,
                ChatId = chatId,
                ActionArgs = args,
                Configuration = _conf,
                Logger = _logger
            };

            return context;
        }
    }
}
