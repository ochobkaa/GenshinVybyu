using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GenshinVybyu.Actions.Utils
{
    public class ActionContext
    {
        public ITelegramBotClient Client { get; init; }
        public IModelCalc ModelCalc { get; init; }
        public BotConfiguration Configuration { get; init; }
        public ChatId ChatId { get; init; }
        public ActionArgs ActionArgs { get; init; }
        public ILogger Logger { get; init; }
    }
}
