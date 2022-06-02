using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary.Settings;

namespace PasswordManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString { get => ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString; }

        App()
        {
            DBConnectionSingleton.ConnectionString = ConnectionString;
            ApplySettings();
        }

        public void ApplySettings()
        {
            WindowSettingsService settingsService = new WindowSettingsService();

            if (!settingsService.IsSavedSettings) return;

            WindowSettings windowSettings = (WindowSettings)settingsService.GetSettings();
            SetLanguage(windowSettings.Language);
            SetColorTheme(windowSettings.Theme);
        }
        private void SetColorTheme(WindowSettings.Themes theme)
        {
            if (theme == WindowSettings.Themes.System)
                theme = ThemesService.GetSystemTheme();

            ResourceDictionary mainDict = new ResourceDictionary { Source = new Uri($"AuthenticationWindow/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(mainDict);
        }
        private void SetLanguage(WindowSettings.Languages language)
        {
            string languageCulture;
            switch (language)
            {
                case WindowSettings.Languages.System:
                    return;
                case WindowSettings.Languages.English:
                    languageCulture = "en-UK";
                    break;
                case WindowSettings.Languages.Ukrainian:
                    languageCulture = "uk-UA";
                    break;
                case WindowSettings.Languages.Russian:
                    languageCulture = "ru-RU";
                    break;
                default:
                    languageCulture = "en-Uk";
                    break;
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCulture);
        }
    }
}
