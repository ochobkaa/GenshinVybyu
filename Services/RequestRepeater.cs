using GenshinVybyu.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;

namespace GenshinVybyu.Services
{
    public class RequestRepeater : IRequestRepeater
    {
        private readonly ILogger _logger;

        public RequestRepeater(ILogger<RequestRepeater> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Repeat(Func<CancellationToken, Task> action, int delay, int times, 
            CancellationToken cancellationToken)
        {
            bool infinite = times == -1;

            bool loop = true;
            int tries = 0;
            while (loop)
            {
                bool triesLessMax = tries < times;
                loop = infinite || triesLessMax;

                tries++;

                _logger.LogDebug($"Requesting... (try {tries})");

                try
                {
                    await action(cancellationToken);

                    _logger.LogDebug("Request sucessful!");

                    return true;
                }
                catch (Exception e) when (e is HttpRequestException || e is RequestException) {
                    if (loop)
                    {
                        _logger.LogDebug($"Request failure: {e.Message} Retry after {delay} ms...");

                        await Task.Delay(delay, cancellationToken);
                    }
                    else
                    {
                        _logger.LogError($"Request failure: {e.Message}");
                    }
                }
            }

            return false;
        }

        public async Task<T?> Repeat<T>(Func<CancellationToken, Task<T>> action, int delay, int times,
            CancellationToken cancellationToken)
        {
            bool infinite = times == -1;

            bool loop = true;
            int tries = 0;
            while (loop)
            {
                tries++;

                bool triesLessMax = tries < times;
                loop = infinite || triesLessMax;

                _logger.LogDebug($"Requesting... (try {tries})");

                try
                {
                    T val = await action(cancellationToken);

                    _logger.LogDebug("Request sucessful!");

                    return val;
                }
                catch (Exception e) when (e is HttpRequestException || e is RequestException)
                {
                    if (loop)
                    {
                        _logger.LogDebug($"Request failure: {e.Message} Retry after {delay} ms...");

                        await Task.Delay(delay, cancellationToken);
                    }
                    else
                    {
                        _logger.LogError($"Request failure: {e.Message}");
                    }
                }

            }

            return default;
        }
    }
}
