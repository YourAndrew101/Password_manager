using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary.Settings;

namespace ServicesLibrary.SettingsService
{
    public class WindowSettingsService : ISettingsService
    {
        public bool IsSavedSettings
        {
            get => !string.IsNullOrEmpty(((WindowSettings)GetSettings()).Language.ToString());
        }

        public void Save(ISettings settings)
        {
            if (settings is WindowSettings newSettings)
            {
                Properties.Settings.Default.Language = newSettings.Language.ToString();
                Properties.Settings.Default.Theme = newSettings.Theme.ToString();
                Properties.Settings.Default.ToTray = newSettings.ToTray;
                Properties.Settings.Default.PasswordGenerateLength = newSettings.PasswordGenerateLength;
                Properties.Settings.Default.Save();
            }
        }

        public void Clear()
        {
            Properties.Settings.Default.Language = "";
            Properties.Settings.Default.Theme = "";
            Properties.Settings.Default.ToTray = false;
            Properties.Settings.Default.PasswordGenerateLength = WindowSettings.StandartPasswordGenerateLength;

            Properties.Settings.Default.Save();
        }

        public ISettings GetSettings()
        {
            Enum.TryParse(Properties.Settings.Default.Language, out WindowSettings.Languages language);
            Enum.TryParse(Properties.Settings.Default.Language, out WindowSettings.Themes theme);

            bool toTray = Properties.Settings.Default.ToTray;
            int passwordGenerateLength = 
                Properties.Settings.Default.PasswordGenerateLength == 0 ? WindowSettings.StandartPasswordGenerateLength : Properties.Settings.Default.PasswordGenerateLength;

            return new WindowSettings(language, theme, toTray, passwordGenerateLength);
        }
    }
}
