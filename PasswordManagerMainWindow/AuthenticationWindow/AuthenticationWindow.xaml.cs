using System;
using System.Collections.Generic;
using System.Linq;
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

            LaunchPreparation();
        }

        private void LaunchPreparation()
        {
            ISettingsService settingsService = new SignUpSettingsService();
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
            ISettingsService settingsService = new WindowSettingsService();

            if (!settingsService.IsSavedSettings) return;

            WindowSettings windowSettings = (WindowSettings)settingsService.GetSettings();
            SetColorTheme(windowSettings.Theme);
        }
        private void SetColorTheme(WindowSettings.Themes theme)
        {
            if (theme == WindowSettings.Themes.System)
                theme = ThemesService.GetSystemTheme();

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
            DateTime[] modifyDateTimes = GetModifeDateTimes(user, dataProviderFactories);
            int lastModifyDateTimeIndex;

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
        private DateTime[] GetModifeDateTimes(User user, CommonDataProviderFactory[] dataProviderFactories)
        {
            DateTime[] modifyDateTimes = new DateTime[dataProviderFactories.Length];

            for (int i = 0; i < dataProviderFactories.Length; i++)
            {
                IDataProvider dataProvider = dataProviderFactories[i].GetDataProvider();
                modifyDateTimes[i] = dataProvider.GetLastModifyTime(user);
            }

            return modifyDateTimes;
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
