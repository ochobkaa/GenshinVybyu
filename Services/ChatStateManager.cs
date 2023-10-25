using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Telegram.Bot.Types;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace GenshinVybyu.Services
{
    public class ChatStateManager : IChatStateManager
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IHashChat _hashChat;

        public ChatStateManager(IConnectionMultiplexer connection, IHashChat hashChat)
        {
            _connection = connection;
            _hashChat = hashChat;
        }

        public async Task<ChatState?> GetState(ChatId chatId)
        {
            IDatabase db = _connection.GetDatabase();

            RedisKey key = _hashChat.Hash(chatId);
            RedisValue state = await db.StringGetAsync(key);
            string stateStr = state.ToString();

            if (stateStr == "nil") return null;

            var chatState = JsonConvert.DeserializeObject<ChatState>(stateStr);

            return chatState;
        }

        public async Task<bool> SetState(ChatId chatId, ChatState state)
        {
            IDatabase db = _connection.GetDatabase();

            RedisKey key = _hashChat.Hash(chatId);
            RedisValue value = JsonConvert.SerializeObject(state);
            bool success = await db.SetAddAsync(key, value);

            return success;
        }
    }
}
