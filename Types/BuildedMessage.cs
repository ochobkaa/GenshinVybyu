using Telegram.Bot.Types.ReplyMarkups;

namespace GenshinVybyu.Types
{
    public class BuildedMessage
    {
        public string Text { get; set; }
        public InlineKeyboardMarkup? Keyboard { get; set; }
    }
}
