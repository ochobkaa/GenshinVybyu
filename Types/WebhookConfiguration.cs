using Newtonsoft.Json;

namespace GenshinVybyu.Types
{
    public class WebhookConfiguration
    {
        public static readonly string Configuration = "WebhookConfiguration";

        public string HostAddress { get; init; } = default!;
        public bool UseNgrok { get; init; } = default!;
        public string Route { get; init; } = default!;
        public int RequestRetryDelay { get; init; } = default!;
        public int RequestRetryTimes { get; init; } = default!;
    }
}
