using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Data.DataProviders.Factories;
using Data.DataProviders.Products;
using PasswordManager.AuthenticationWindow.Pages;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary;
using UsersLibrary.Services;
using UsersLibrary.Settings;
using static UsersLibrary.UsersExceptions;

namespace PasswordManager.AuthenticationWindow
{
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
            SetConnectionDataBase();           
            LaunchPreparation();
        }

        private void SetConnectionDataBase()
        {
            UsersService.ConnectionString = App.ConnectionString;
        }

        private void LaunchPreparation()
        {
            SignUpSettingsService settingsService = new SignUpSettingsService();
            if (!settingsService.IsSavedSettings)
            {
                StartAuthenticationWindow();
                return;
            }

            SignUpSettings settings = (SignUpSettings)settingsService.GetSettings();
            User user = User.CreateAlreadyExistUser(settings.Email, settings.AuthPassword);

            if (InternetService.IsConnectedToInternet)
                GetUserDataFromDB(user);
            else
                GetUserDataFromLocalStorage(user);
        }     

        private void GetUserDataFromDB(User user)
        {
            try
            {
                user = UsersService.GetHashAndSaltFromDB(user);
                if (UsersService.CheckUserData(user))
                {
                    StartMainWindow(user);
                    return;
                }
            }
            catch (IncorrectPasswordException)
            {
                StartAuthenticationWindow();
                return;
            }
        }
        private void GetUserDataFromLocalStorage(User user)
        {
            StartMainWindow(user);
        }


        private void StartAuthenticationWindow()
        {
            SetStartUpPage();
            ApplySettings();
        }
        private void SetStartUpPage()
        {
            AuthFrame.Content = new Login();
        }
        private void ApplySettings()
        {
            WindowSettingsService settingsService = new WindowSettingsService();

            if (!settingsService.IsSavedSettings) return;

            WindowSettings windowSettings = (WindowSettings)settingsService.GetSettings();
            SetLanguage(windowSettings.Language);
            SetColorTheme(windowSettings.Theme);
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
        private void SetColorTheme(WindowSettings.Themes theme)
        {
            if (theme == WindowSettings.Themes.System)
                theme = ThemesService.GetSystemTheme();
            ResourceDictionary mainDict = new ResourceDictionary { Source = new Uri($"MainWindow/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(mainDict);
            ResourceDictionary authDict = new ResourceDictionary { Source = new Uri($"AuthenticationWindow/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(authDict);

        }
        public void StartMainWindow(User user)
        {
            GetServicesData(user);

            MainWindow.MainWindow mainWindow = new MainWindow.MainWindow(user);
            mainWindow.Show();

            CloseWindow(new object(), new RoutedEventArgs());

            return;
        }
        private void GetServicesData(User user)
        {
            CommonDataProviderFactory[] dataProviderFactories = CommonDataProviderFactory.CreateFactories();
            DateTime[] modifyDateTimes = new DateTime[dataProviderFactories.Length];
            int lastModifyDateTimeIndex;

            for (int i = 0; i < dataProviderFactories.Length; i++)
            {
                IDataProvider dataProvider = dataProviderFactories[i].GetDataProvider();
                modifyDateTimes[i] = dataProvider.GetLastModifyTime(user);
            }

            lastModifyDateTimeIndex = modifyDateTimes.ToList().IndexOf(modifyDateTimes.Max());
            List<Service> services = dataProviderFactories[lastModifyDateTimeIndex].GetDataProvider().Load(user);
            user.Services = services;

            for (int i = 0; i < dataProviderFactories.Length; i++)
            {
                if (i == lastModifyDateTimeIndex) continue;
                IDataProvider dataProvider = dataProviderFactories[i].GetDataProvider();

                dataProvider.Clear(user);
                dataProvider.Save(user, services);
            }
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
