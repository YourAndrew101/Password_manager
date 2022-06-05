using UsersLibrary.Settings;

namespace ServicesLibrary.SettingsService
{
    public class WindowSettingsService : ISettingsService
    {
        public bool IsSavedSettings
        {
            get => !string.IsNullOrEmpty(((WindowSettings)GetSettings()).Language.ToString());
        }

        public void SaveSettings(ISettings settings)
        {
            if (settings is WindowSettings newSettings)
            {
                Properties.Settings.Default.Language = (int)newSettings.Language;
                Properties.Settings.Default.Theme = (int)newSettings.Theme;
                Properties.Settings.Default.ToTray = newSettings.ToTray;
                Properties.Settings.Default.PasswordGenerateLength = newSettings.PasswordGenerateLength;
                Properties.Settings.Default.Save();
            }
        }

        public void ClearSettings()
        {
            Properties.Settings.Default.Language = 0;
            Properties.Settings.Default.Theme = 0;
            Properties.Settings.Default.ToTray = false;
            Properties.Settings.Default.PasswordGenerateLength = WindowSettings.StandartPasswordGenerateLength;

            Properties.Settings.Default.Save();
        }

        public ISettings GetSettings()
        {
            WindowSettings.Languages language = (WindowSettings.Languages)Properties.Settings.Default.Language;
            WindowSettings.Themes theme = (WindowSettings.Themes)Properties.Settings.Default.Theme;

            bool toTray = Properties.Settings.Default.ToTray;
            int passwordGenerateLength = 
                Properties.Settings.Default.PasswordGenerateLength == 0 ? WindowSettings.StandartPasswordGenerateLength : Properties.Settings.Default.PasswordGenerateLength;

            return new WindowSettings(language, theme, toTray, passwordGenerateLength);
        }
    }
}
