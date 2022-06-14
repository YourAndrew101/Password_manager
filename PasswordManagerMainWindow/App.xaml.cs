using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary.Settings;

namespace PasswordManager
{
    public partial class App : Application
    {
        public static string ConnectionString { get => ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString; }

        public MainWindow.MainWindow GetMainWindow
        {
            get
            {
                foreach (Window window in Windows)
                    if (window is MainWindow.MainWindow mainWindow) return mainWindow;

                return null;
            }
        }

        App()
        {
            DBConnectionSingleton.ConnectionString = ConnectionString;
            ApplySettings();
        }
        public void ApplySettings()
        {
            ISettingsService settingsService = new WindowSettingsService();

            if (!settingsService.IsSavedSettings) return;

            WindowSettings windowSettings = (WindowSettings)settingsService.GetSettings();
            SetLanguage(windowSettings.Language);
            
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


        public void ExitUser()
        {
            this.Windows[0]
        }
    }
}
