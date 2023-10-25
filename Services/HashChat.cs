using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace GenshinVybyu.Services
{
    public class HashChat : IHashChat
    {
        private readonly SensitiveData _sensitive;

        public HashChat(IOptions<SensitiveData> sensitive)
        {
            _sensitive = sensitive.Value;
        }

        public string Hash(ChatId chatId)
        {
            string chatIdStr = chatId.ToString();
            string seed = _sensitive.UserHashSeed;
            string token = _sensitive.SecretToken;

            string toHash = $"{seed}{chatIdStr}{token}";
            byte[] toHashBytes = Encoding.UTF8.GetBytes(toHash);
            byte[] hashBytes = SHA256.HashData(toHashBytes);
            string hash = BitConverter.ToString(hashBytes)
                .Replace("-", "")
                .ToLower();

            return hash;
        }
    }
}
