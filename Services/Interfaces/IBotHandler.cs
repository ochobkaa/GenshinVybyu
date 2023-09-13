using Telegram.Bot.Types;

namespace GenshinVybyu.Services.Interfaces
{
    public interface IBotHandler
    {
        public Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
        public Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken);
    }
}