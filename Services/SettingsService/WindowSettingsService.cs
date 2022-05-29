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
            get => !string.IsNullOrEmpty(((SignUpSettings)GetSettings()).Email);
        }

        public void Save(ISettings settings)
        {
            if (settings is WindowSettings newSettings)
            {
                Properties.Settings.Default.Language = newSettings.Language.ToString();
                Properties.Settings.Default.Theme = newSettings.Theme.ToString();
                Properties.Settings.Default.IsTray = newSettings.ToTrey;
                Properties.Settings.Default.Save();
            }
        }

        public void Clear()
        {
            Properties.Settings.Default.Language = "";
            Properties.Settings.Default.Theme = "";
            Properties.Settings.Default.IsTray = false;
            Properties.Settings.Default.Save();
        }

        public ISettings GetSettings()
        {
            WindowSettings.Languages language = (WindowSettings.Languages)Enum.Parse(typeof(WindowSettings.Languages), Properties.Settings.Default.Language);
            WindowSettings.Themes theme = (WindowSettings.Themes)Enum.Parse(typeof(WindowSettings.Themes), Properties.Settings.Default.Theme);
            bool toTray = Properties.Settings.Default.IsTray;

            return new WindowSettings(language, theme, toTray);
        }
    }
}
