using Ngrok.AgentAPI;
using Microsoft.Extensions.Options;
using GenshinVybyu.Types;
using GenshinVybyu.Services.Interfaces;

namespace GenshinVybyu.Services
{
    public class NgrokTunnel : INgrokTunnel
    {
        private readonly NgrokAgentClient _ngrok;
        private readonly IRequestRepeater _repeater;
        private readonly WebhookConfiguration _webhookConfig;
        private readonly ILogger<NgrokTunnel> _logger;

        private bool _isActive = false;

        public bool IsActive => _isActive;
        public string TunnelName = "GenshinVybyu";

        public NgrokTunnel(
            NgrokAgentClient ngrok,
            IRequestRepeater repeater,
            IOptions<WebhookConfiguration> webhookConfig, 
            ILogger<NgrokTunnel> logger)
        {
            _ngrok = ngrok;
            _repeater = repeater;
            _webhookConfig = webhookConfig.Value;
            _logger = logger;
        }

        public async Task<string?> CreateTunnel(string uri, CancellationToken cancellationToken)
        {
            int retryDelay = _webhookConfig.RequestRetryDelay;
            int retryTimes = _webhookConfig.RequestRetryTimes;

            _logger.LogDebug(uri);

            var tunnelCreator = async (CancellationToken ct) =>
            {
                var ngrokConf = new HttpTunnelConfiguration(TunnelName, uri);
                TunnelDetail tunnel = await _ngrok.StartTunnelAsync(ngrokConf, ct);
                _isActive = true;

                string address = tunnel.PublicUrl;
                return address;
            };

            _logger.LogInformation("Creating Ngrok tunnel...");

            string? address = await _repeater.Repeat(tunnelCreator, retryDelay, retryTimes, cancellationToken);
           
            if (address is not null)
            {
                _logger.LogInformation($"Ngrok tunnel is available at: {address}");

                _isActive = true;
            }
            else
                _logger.LogError($"Failed to open Ngrok tunnel");

            return address;
        }

        public async Task CloseTunnel(CancellationToken cancellationToken)
        {
            if (!_isActive)
                return;

            int retryDelay = _webhookConfig.RequestRetryDelay;
            int retryTimes = _webhookConfig.RequestRetryTimes;

            var tunnelCloser = async (CancellationToken ct) => 
            {
                await _ngrok.StopTunnelAsync(TunnelName, ct);
            };

            _logger.LogDebug("Closing Ngrok tunnel...");

            bool success = await _repeater.Repeat(tunnelCloser, retryDelay, retryTimes, cancellationToken);

            if (success)
            {
                _logger.LogInformation("Ngrok tunnel is closed");

                _isActive = false;
            }
            else
                _logger.LogError($"Failed to close Ngrok tunnel");
        }
    }
}
