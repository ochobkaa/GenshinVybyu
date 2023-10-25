using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IBotOutput : IService
    {
        public Task Message(
            ChatId chatId,
            string messageName,
            CancellationToken cancellationToken
        );
    }
}
