using GenshinVybyu.Services;
using GenshinVybyu.Services.Interfaces;
using Ngrok.AgentAPI;

namespace GenshinVybyu
{
    public static class NgrokExtensions
    {
        public static IServiceCollection AddNgrokTunnel(this IServiceCollection services)
        {
            string address = "http://ngrok:4040/api";

            services
                .AddSingleton(provider => new NgrokAgentClient(address))
                .AddTransient<INgrokTunnel, NgrokTunnel>();

            return services;
        }
    }
}
