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
            SetConnectionDataBase();
            //SettingsService.SaveEmptySignUpSettings();

            LaunchPreparation();
        }

        private void SetConnectionDataBase()
        {
            UsersService.ConnectionString = App.ConnectionString;
        }

        private void LaunchPreparation()
        {
            SignUpSettingsService settingsService = new SignUpSettingsService();
            if (!settingsService.IsSavedUser)
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
            SetSystemColorTheme();
        }
        private void SetStartUpPage()
        {
            AuthFrame.Content = new Login();
        }
        private void SetSystemColorTheme()
        {
            WindowSettings.Themes theme = ThemesService.GetSystemTheme();

            ResourceDictionary dict = new ResourceDictionary { Source = new Uri($"AuthenticationWindow/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(dict);
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
