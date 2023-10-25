using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Types;
using Microsoft.Extensions.Options;

namespace GenshinVybyu.Services
{
    public class SplashGenerator : ISplashGenerator
    {
        private readonly BotConfiguration _botConfig;
        private readonly MessagesConfig _msgConfig;

        public SplashGenerator(
            IOptions<BotConfiguration> botConfig,
            IOptions<MessagesConfig> msgConfig
        )
        {
            _botConfig = botConfig.Value;
            _msgConfig = msgConfig.Value;
        }

        public Splash? GetSplash()
        {
            IList<Splash> splashes = _msgConfig.Splashes;

            var generator = new Random(Environment.TickCount);
            double diceRoll = generator.NextDouble();

            double splashProb = _botConfig.SplashProb;
            bool genSplash = diceRoll < splashProb;

            if (genSplash)
            {
                int splashNum = generator.Next(splashes.Count);
                Splash splash = splashes[splashNum];
                return splash;
            }
            else
                return null;
        }
    }
}
