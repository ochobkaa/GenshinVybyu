using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using GenshinVybyu.Services.Interfaces;

namespace GenshinVybyu.Services
{
    public class BotHandler : IBotHandler
    {
        private readonly IActionsHandler<Message> _messageHandler;
        private readonly IActionsHandler<CallbackQuery> _cbQueryHandler;
        private readonly ILogger _logger;

        public BotHandler(
            IActionsHandler<Message> messageHandler,
            IActionsHandler<CallbackQuery> cbQueryHandler,
            ILogger<BotHandler> logger
        )
        {
            _messageHandler = messageHandler;
            _cbQueryHandler = cbQueryHandler;
            _logger = logger;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => _OnMessageReceived(message, cancellationToken),
                { EditedMessage: { } message } => _OnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => _OnCallbackQueryReceived(callbackQuery, cancellationToken),
                { InlineQuery: { } inlineQuery } => _OnInlineQueryReceived(inlineQuery, cancellationToken),
                { ChosenInlineResult: { } chosenInlineResult } => _OnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
                _ => _UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }

        private async Task _OnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
                return;

            await _messageHandler.Handle(message, cancellationToken);
        }

        // Process Inline Keyboard callback data
        private async Task _OnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

            await _cbQueryHandler.Handle(callbackQuery, cancellationToken);
        }

        private async Task _OnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
        {
            // пока что TODO
            _logger.LogDebug("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

            await Task.CompletedTask;
        }

        private async Task _OnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
        {
            // пока что TODO
            _logger.LogDebug("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);

            await Task.CompletedTask;
        }

        private async Task _UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Unknown update type: {UpdateType}", update.Type);

            await Task.CompletedTask;
        }

        public async Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError("HandleError: {ErrorMessage}", ErrorMessage);

            // Cooldown in case of network connection error
            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}