using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Actions.Utils;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace GenshinVybyu.Services
{
    public class BotOutput : IBotOutput
    {
        private readonly ITelegramBotClient _client;
        private readonly IServiceProvider _provider;
        private readonly IMessageBuilder _builder;
        public BotOutput(
            ITelegramBotClient client, 
            IServiceProvider provider,
            IMessageBuilder builder
        )
        {
            _client = client;
            _provider = provider;
            _builder = builder;
        }
        
        public async Task Message(
            ChatId chatId, 
            string messageName, 
            CancellationToken cancellationToken,
            bool addSplash = false,
            IDictionary<string, string>? replaces = null,
            RollsData? rollsData = null
        )
        {
            BuildedMessage? buildedMsg = _builder.BuildMessage(messageName, addSplash, replaces, rollsData);

            if (buildedMsg == null) return;

            string text = buildedMsg.Text;
            InlineKeyboardMarkup? keyboard = buildedMsg.Keyboard;
            await _client.SendTextMessageAsync(
                chatId, text,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}
