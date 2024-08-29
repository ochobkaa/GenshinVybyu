using GenshinVybyu.Types;
using StackExchange.Redis;

namespace GenshinVybyu
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, SensitiveData sensitiveData)
        {
            string connectionString = sensitiveData.RedisConnectionString;
            var connection = ConnectionMultiplexer.Connect(connectionString);

            services.AddSingleton<IConnectionMultiplexer>(connection);
            return services;
        }
    }
}
