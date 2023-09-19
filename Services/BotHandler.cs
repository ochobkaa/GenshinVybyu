using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using GenshinVybyu.Services.Interfaces;

namespace GenshinVybyu.Services
{
    public class BotHandler : IBotHandler
    {
        private readonly ITelegramBotClient _client;
        private readonly ILogger _logger;

        public BotHandler(ITelegramBotClient client, ILogger logger)
        {
            _client = client;
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


        }

        // Process Inline Keyboard callback data
        private async Task _OnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

            await _client.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}",
                cancellationToken: cancellationToken);

            await _client.SendTextMessageAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                text: $"Received {callbackQuery.Data}",
                cancellationToken: cancellationToken);
        }

        #region Inline Mode

        private async Task _OnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "1",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent("hello"))
        };

            await _client.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                cacheTime: 0,
                isPersonal: true,
                cancellationToken: cancellationToken);
        }

        private async Task _OnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);

            await _client.SendTextMessageAsync(
                chatId: chosenInlineResult.From.Id,
                text: $"You chose result with Id: {chosenInlineResult.ResultId}",
                cancellationToken: cancellationToken);
        }

        #endregion

        private Task _UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
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