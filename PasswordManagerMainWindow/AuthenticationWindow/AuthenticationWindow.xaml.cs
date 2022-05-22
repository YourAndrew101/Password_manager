using System;
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
using PasswordManager.AuthenticationWindow.Pages;
using ServicesLibrary;
using UsersLibrary;
using UsersLibrary.Settings;

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
            if (!SettingsService.IsSavedUser)
            {
                StartAuthenticationWindow();
                return;
            }

            SignUpSettings settings = SettingsService.GetSignUpSettings();
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
                if(UsersService.CheckUserData(user))
                {
                    StartMainWindow(user);
                    return;
                }
            }
            catch (Exception)
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
