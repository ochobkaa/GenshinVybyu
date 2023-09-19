namespace GenshinVybyu.Types
{
    public class BotConfiguration
    {
        public static readonly string Configuration = "BotConfiguration";

        public string ModelDataPath { get; set; } = default!;
        public string ModelDataFilenameFormat { get; set; } = default!;
        public int RollPrimogemsCost { get; set; } = default!;
        public int CharPity { get; set; } = default!;
        public int WeaponPity { get; set; } = default!;
    }
}
