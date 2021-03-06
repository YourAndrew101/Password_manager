namespace UsersLibrary.Settings
{
    public class WindowSettings : ISettings
    {
        public static readonly int StandartPasswordGenerateLength = 16;

        public enum Languages
        {
            System = 0,
            English = 1,
            Ukrainian = 2,
            Russian = 3,
        }

        public enum Themes
        {
            System = 0, 
            Dark = 1,
            Light = 2
        }

        public Languages Language { get; set; }
        public Themes Theme { get; set; }
        public bool ToTray { get; set; }
        public int PasswordGenerateLength { get; set; }

        public WindowSettings() { }

        public WindowSettings(Languages language, Themes theme, bool toTray, int passwordGenerateLength )
        {
            Language = language;
            Theme = theme;
            ToTray = toTray;
            PasswordGenerateLength = passwordGenerateLength;
        }
    }
}
