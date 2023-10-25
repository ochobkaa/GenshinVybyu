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
        private readonly IBotOutput _output;
        private readonly IModelCalc _calc;
        private readonly IChatStateActions _state;
        private readonly BotConfiguration _conf;
        private readonly ILogger _logger;

        public ActionExecutor(
            IBotOutput output,
            IModelCalc calc,
            IChatStateActions state,
            IOptions<BotConfiguration> options,
            ILogger<ActionExecutor> logger
        )
        {
            _output = output;
            _calc = calc;
            _state = state;
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
                Output = _output,
                ModelCalc = _calc,
                State = _state,
                ChatId = chatId,
                ActionArgs = args,
                Configuration = _conf,
                Logger = _logger
            };

            return context;
        }
    }
}
