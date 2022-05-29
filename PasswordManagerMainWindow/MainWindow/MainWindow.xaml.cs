using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PasswordManager.MainWindow.Pages;
using ServicesLibrary;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.MainWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {  
        private User User { get; set; }
        private Home home;
        private Settings settings;
        private Account account;
            
        public MainWindow(User user)
        {
            User = user;
            home = new Home(User);                 
            settings = new Settings();
            account = new Account();                        
            InitializeComponent();
            SetSystemColorTheme();
            SetWindowSettings();
        }

        private void SetWindowSettings()
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            WindowState = WindowState.Normal;
            Home.IsChecked = true;
        }

        //TODO додати різні теми
        private void SetSystemColorTheme()
        {
            WindowSettings.Themes theme = ThemesService.GetSystemTheme();

            ResourceDictionary dict = new ResourceDictionary { Source = new Uri($"/MainWindow/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }


        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeWindow_Checked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void MaximizeWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Maximized && MaximizeWindow.IsChecked == false)
                MaximizeWindow.IsChecked = true;
            if (WindowState == WindowState.Normal && MaximizeWindow.IsChecked == true)
                MaximizeWindow.IsChecked = false;
        }
        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            Animation.NavMenuAnimation(Axis, 0);
            MainFrame.Content = home;
        }

        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            Animation.NavMenuAnimation(Axis, 504);
            MainFrame.Content = settings;
        }

        private void Account_Checked(object sender, RoutedEventArgs e)
        {
            Animation.NavMenuAnimation(Axis, 1008);           
            MainFrame.Content = account;
        }

        
        public void ShowNotification(string NotificationMessage)
        {
            Animation.ShowNotificationAnimation(NotificationBody);
            NotificationText.Text = NotificationMessage;
        }
    }
}
