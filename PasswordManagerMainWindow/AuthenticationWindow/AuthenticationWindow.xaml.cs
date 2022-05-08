using System;
using System.Collections.Generic;
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

namespace PasswordManager.AuthenticationWindow
{
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();

            //SettingsService.SaveEmptySettings();

            LaunchPreparation();      
        }

        private void LaunchPreparation()
        {
            User user;
            if (SettingsService.IsSavedUser)
            {
                user = (User)SettingsService.GetSettings();
                StartMainWindow(user);
            }
            else
            {
                SetStartUpPage();
                SetSystemColorTheme();
            }

            //if(InternetService.IsConnectedToInternet)

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
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
