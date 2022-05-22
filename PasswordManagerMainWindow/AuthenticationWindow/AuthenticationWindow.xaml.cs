﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Data;
using Data.DataProviders.Factories;
using Data.DataProviders.Products;
using PasswordManager.AuthenticationWindow.Pages;
using ServicesLibrary;
using UsersLibrary;
using UsersLibrary.Services;
using UsersLibrary.Settings;
using static UsersLibrary.UsersExceptions;

namespace PasswordManager.AuthenticationWindow
{
    public partial class AuthenticationWindow : Window
    {
        private IEnumerable<Service> Services = new List<Service>() 
        {
            new Service("testName", "testLogin", "testPassword") { Id = 1},
            new Service("testName1", "testLogin1", "testPassword1") { Id = 2},
            new Service("testName2", "testLogin2", "testPassword2") { Id = 3},
            new Service("testName3", "testLogin3", "testPassword3") { Id = 4},
            new Service("testName4", "testLogin4", "testPassword4") { Id = 5},
            new Service("testName5", "testLogin5", "testPassword5") { Id = 6},
            new Service("testName6", "testLogin6", "testPassword6") { Id = 7},
        };

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
            if (!SettingsService.IsSavedUser)
            {
                StartAuthenticationWindow();
                return;
            }

            SignUpSettings settings = SettingsService.GetSignUpSettings();
            User user = User.CreateAlreadyExistUser(settings.Email, settings.AuthPassword);

            IDataProvider dataProvider = new SQLDataProvider();
            dataProvider.Save(user, Services);


            GetServicesData(user);

            if (InternetService.IsConnectedToInternet)
                GetUserDataFromDB(user);
            else
                GetUserDataFromLocalStorage(user);
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

                dataProviderFactories[i].GetDataProvider().Save(user, services);
            }
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
            throw new NotImplementedException();
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
            ThemesService.Themes theme = ThemesService.GetSystemTheme();

            ResourceDictionary dict = new ResourceDictionary { Source = new Uri($"AuthenticationWindow/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }


        public void StartMainWindow(User user)
        {
            MainWindow.MainWindow mainWindow = new MainWindow.MainWindow(user);
            mainWindow.Show();

            CloseWindow(new object(), new RoutedEventArgs());

            return;
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
