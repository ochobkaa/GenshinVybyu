using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot.Types;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace GenshinVybyu.Services
{
    public class ChatStateManager : IChatStateManager
    {
        public delegate T? StrToValConverter<T>(string valStr);
        public delegate string ValToStrConverter<T>(T val);

        public string ChatPrefix => "chat";

        private readonly IConnectionMultiplexer _connection;
        private readonly IHashChat _hashChat;

        public ChatStateManager(IConnectionMultiplexer connection, IHashChat hashChat)
        {
            _connection = connection;
            _hashChat = hashChat;
        }

        private RedisKey GetKey(ChatId chatId, string paramName)
        {
            string hash = _hashChat.Hash(chatId);
            RedisKey key = $"{ChatPrefix}:{hash}:{paramName}";

            return key;
        }

        public async Task<T?> GetState<T>(ChatId chatId, string paramName, 
            StrToValConverter<T?> conv)
        {
            IDatabase db = _connection.GetDatabase();

            RedisKey key = GetKey(chatId, paramName);
            string? valStr = await db.StringGetAsync(key);
            if (valStr is null)
                return default;

            T? val = conv(valStr);

            return val;
        }

        public async Task<bool> SetState<T>(ChatId chatId, string paramName, T val,
            ValToStrConverter<T>? conv = null)
        {
            IDatabase db = _connection.GetDatabase();

            RedisKey key = GetKey(chatId, paramName);
            RedisValue valStr = conv switch
            {
                null => val.ToString(),
                { } => conv(val)
            };

            bool success = await db.StringSetAsync(key, valStr);
            return success;
        }

        public async Task<bool> DeleteState(ChatId chatId, string paramName)
        {
            IDatabase db = _connection.GetDatabase();

            RedisKey key = GetKey(chatId, paramName);

            bool success = await db.KeyDeleteAsync(key);
            return success;
        }
    }
}
