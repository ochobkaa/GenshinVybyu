using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using GenshinVybyu.Actions.Utils;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types.ReplyMarkups;


namespace GenshinVybyu.Services
{
    public class MessageBuilder : IMessageBuilder
    {
        private readonly ISplashGenerator _splashes;
        private readonly IMessageTextReplacer _replacer;
        private readonly BotConfiguration _botConf;
        private readonly MessagesConfig _msgConfig;

        public MessageBuilder(
            ISplashGenerator splashes,
            IMessageTextReplacer replacer,
            IOptions<BotConfiguration> botConf,
            IOptions<MessagesConfig> msgConfig
        )
        {
            _splashes = splashes;
            _replacer = replacer;
            _botConf = botConf.Value;
            _msgConfig = msgConfig.Value;
        }

        private string GetMessageText(BotMessage msg, bool addSplash)
        {
            string msgText = msg.Text;

            if (addSplash)
            {
                Splash? splash = _splashes.GetSplash();
                if (splash != null)
                {
                    string splashText = splash.Label;
                    msgText = $"{msgText}\n\n{splashText}";
                }
            }

            return msgText;
        }

        private string GetCommand(BotKeyboardButton button)
        {
            string commandToken = button.Command ?? "";

            List<string>? args = button.Args?.ToList();
            Dictionary<string, string>? kwArgs = button.KwArgs?
                .ToDictionary(b => b.Name, b => b.Value);

            var parsedCommand = new ParsedCommand()
            {
                Token = commandToken,
                Args = new() { Args = args, KwArgs = kwArgs },
                CommandPrefix = _botConf.CommandPrefix,
                KeyAttrValuePrefix = _botConf.KeyAttrValuePrefix
            };

            string commandStr = parsedCommand.ToString();
            return commandStr;
        }

        private InlineKeyboardButton CreateButton(BotKeyboardButton button)
        {
            string label = button.Label;

            InlineKeyboardButton inlineButton;
            string buttonType = button.Type;
            if (buttonType == "command")
            {
                string commandStr = GetCommand(button);

                inlineButton = InlineKeyboardButton.WithCallbackData(label, commandStr);
            }
            else
            {
                string message = button.Message ?? "";

                inlineButton = InlineKeyboardButton.WithCallbackData(label, message);
            }

            return inlineButton;
        }

        private InlineKeyboardMarkup? GetKeyboard(BotMessage msg)
        {
            IEnumerable<BotKeyboardButton>? buttons = msg.Keyboard;

            if (buttons == null) return null;

            var inlineKeyboard = new InlineKeyboardMarkup(
                buttons.Select(b => CreateButton(b))
            );

            return inlineKeyboard;
        }

        private BotMessage? MessageFromConfig(string messageName)
            => _msgConfig.Messages
                .FirstOrDefault(m => m.Name == messageName);

        public BuildedMessage? BuildMessage(
            string messageName,
            bool addSplash,
            IDictionary<string, string>? replaces,
            RollsData? rollsData
        )
        {
            BotMessage? msg = MessageFromConfig(messageName);

            if (msg == null) return null;

            string msgText = GetMessageText(msg, addSplash);

            if (replaces != null && replaces.Count > 0)
                msgText = _replacer.ReplaceText(msgText, replaces);

            if (rollsData != null)
                msgText = _replacer.InsertRollsData(msgText, rollsData);

            InlineKeyboardMarkup? msgKeyboard = GetKeyboard(msg);

            var buildedMsg = new BuildedMessage()
            {
                Text = msgText,
                Keyboard = msgKeyboard
            };
            return buildedMsg;
        }
    }
}
