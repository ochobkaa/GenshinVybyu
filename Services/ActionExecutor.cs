using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Actions.Interfaces;
using GenshinVybyu.Actions.Utils;
using GenshinVybyu.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using StackExchange.Redis;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class ActionExecutor : IActionExecutor
    {
        private readonly IBotOutput _output;
        private readonly IModelCalc _calc;
        private readonly IChatStateActions _state;
        private readonly IRequestRepeater _repeater;
        private readonly BotConfiguration _conf;
        private readonly ILogger _logger;

        public ActionExecutor(
            IBotOutput output,
            IModelCalc calc,
            IChatStateActions state,
            IRequestRepeater repeater,
            IOptions<BotConfiguration> options,
            ILogger<ActionExecutor> logger
        )
        {
            _output = output;
            _calc = calc;
            _state = state;
            _repeater = repeater;
            _conf = options.Value;
            _logger = logger;
        }

        public async Task Exec(
            ChatId chatId,
            string actionName,
            Func<ActionContext, CancellationToken, Task> actionCb, 
            ActionArgs args, 
            CancellationToken cancellationToken
        )
        {
            ActionContext context = CreateActionContext(chatId, actionName, args);

            int retryDelay = _conf.ExecutorRetryDelay;
            int retryTimes = _conf.ExecutorRetryTimes;

            var actionCbExec = async (CancellationToken ct) =>
            {
                await actionCb(context, ct);
            };

            _logger.LogDebug($"Action {actionName} performing...");

            bool success = await _repeater.Repeat(actionCbExec, retryDelay, retryTimes, cancellationToken);

            if (success)
                _logger.LogDebug($"Action {actionName} completed");

            else
                _logger.LogError($"Failed to peform {actionName}");
        }

        private ActionContext CreateActionContext(ChatId chatId, string actionName, ActionArgs args)
        {
            var context = new ActionContext()
            {
                ActionName = actionName,
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
