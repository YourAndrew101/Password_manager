using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows;
using Data.DataProviders.Products;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary;
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
        public AuthenticationWindow.AuthenticationWindow GetAuthenticationWindow
        {
            get
            {
                foreach (Window window in Windows)
                    if (window is AuthenticationWindow.AuthenticationWindow authenticationWindow) return authenticationWindow;

                return null;
            }
        }

        private ISettingsService settingsService;

        App()
        {
            DBConnectionSingleton.ConnectionString = ConnectionString;
            ApplySettings();
        }
        public void ApplySettings()
        {
            settingsService = new WindowSettingsService();

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


        public void ExitUser(User user)
        {
            ClearAllSettings();
            ClearAllServices(user);

            AuthenticationWindow.AuthenticationWindow authenticationWindow = new AuthenticationWindow.AuthenticationWindow();
            authenticationWindow.Show();

            GetMainWindow.DeleteNotificationIcon();
            GetMainWindow.Close();
        }
        private void ClearAllSettings()
        {
            settingsService = new WindowSettingsService();
            settingsService.ClearSettings();
            settingsService = new SignUpSettingsService();
            settingsService.ClearSettings();
        }
        private void ClearAllServices(User user)
        {
            XMLDataProvider xMLDataProvider = new XMLDataProvider();
            xMLDataProvider.DeleteFile(user);
        }
    }
}
