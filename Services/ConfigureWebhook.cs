using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GenshinVybyu.Services
{
    public class ConfigureWebhook : IConfigureWebhook
    {
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WebhookConfiguration _webhookConfig;
        private readonly SensitiveData _sensitiveData;

        public ConfigureWebhook(
            ILogger<ConfigureWebhook> logger,
            IServiceProvider serviceProvider,
            IOptions<WebhookConfiguration> webhookConfig,
            IOptions<SensitiveData> sensitiveData)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _webhookConfig = webhookConfig.Value;
            _sensitiveData = sensitiveData.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Configure custom endpoint per Telegram API recommendations:
            // https://core.telegram.org/bots/api#setwebhook
            // If you'd like to make sure that the webhook was set by you, you can specify secret data
            // in the parameter secret_token. If specified, the request will contain a header
            // "X-Telegram-Bot-Api-Secret-Token" with the secret token as content.

            var webhookAddress = $"{_webhookConfig.HostAddress}{_webhookConfig.Route}";

            _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);

            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                secretToken: _sensitiveData.SecretToken,
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Remove webhook on app shutdown
            _logger.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
