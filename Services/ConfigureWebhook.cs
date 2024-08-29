using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace GenshinVybyu.Services
{
    public class ConfigureWebhook : IConfigureWebhook
    {
        private readonly IRequestRepeater _repeater;
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WebhookConfiguration _webhookConfig;
        private readonly SensitiveData _sensitiveData;

        public ConfigureWebhook(
            IRequestRepeater repeater,
            ILogger<ConfigureWebhook> logger,
            IServiceProvider serviceProvider,
            IOptions<WebhookConfiguration> webhookConfig,
            IOptions<SensitiveData> sensitiveData)
        {
            _repeater = repeater;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _webhookConfig = webhookConfig.Value;
            _sensitiveData = sensitiveData.Value;
        }

        private async Task<string?> SetupNgrok(string address, string route, CancellationToken cancellationToken)
        {
            var ngrokTunnel = _serviceProvider.GetRequiredService<INgrokTunnel>();

            string? ngrokAddress = await ngrokTunnel.CreateTunnel(address, cancellationToken);
            if (ngrokAddress is not null)
            {
                string webhookAddress = $"{ngrokAddress}{route}";
                return webhookAddress;
            }
            else
            {
                return null;
            }
        }

        private async Task SetupWebhook(string webhookAddress, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);

            int retryDelay = _webhookConfig.RequestRetryDelay;
            int retryTimes = _webhookConfig.RequestRetryTimes;

            var webhookSetter = async (CancellationToken ct) => 
            {
                await botClient.SetWebhookAsync(
                    url: webhookAddress,
                    allowedUpdates: Array.Empty<UpdateType>(),
                    secretToken: _sensitiveData.SecretToken,
                    cancellationToken: ct
                );
            };

            bool success = await _repeater.Repeat(webhookSetter, retryDelay, retryTimes, cancellationToken);

            if (success)
                _logger.LogInformation("Webhook is alive!");

            else
                _logger.LogError("Failed to set webhook");
        }

        public async Task DeleteWebhook(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            int retryDelay = _webhookConfig.RequestRetryDelay;
            int retryTimes = _webhookConfig.RequestRetryTimes;

            var webhookRemover = async (CancellationToken ct) =>
            {
                await botClient.DeleteWebhookAsync(cancellationToken: ct);
            };

            bool success = await _repeater.Repeat(webhookRemover, retryDelay, retryTimes, cancellationToken);

            if (success)
                _logger.LogInformation("Webhook deleted!");

            else
                _logger.LogError("Failed to delete webhook");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string address = _webhookConfig.HostAddress;
            string route = _webhookConfig.Route;
            var webhookAddress = $"{address}{route}";

            bool useNgrok = _webhookConfig.UseNgrok;
            if (useNgrok)
            {
                string? ngrokAddress = await SetupNgrok(address, route, cancellationToken);
                if (ngrokAddress is not null)
                    webhookAddress = ngrokAddress;

                else
                    return;
            }

            await SetupWebhook(webhookAddress, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await DeleteWebhook(cancellationToken);

            bool useNgrok = _webhookConfig.UseNgrok;
            if (useNgrok)
            {
                var ngrokTunnel = _serviceProvider.GetRequiredService<INgrokTunnel>();

                await ngrokTunnel.CloseTunnel(cancellationToken);
            }
        }
    }
}
