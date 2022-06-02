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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PasswordManager.MainWindow.Pages;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
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
        public Home HomePage { get; set; }
        public Settings SettingsPage { get; set; }
        public Account AccountPage { get; set; }
            
        public MainWindow(User user)
        {
            User = user;
            HomePage = new Home(User);
            SettingsPage = new Settings(User);
            AccountPage = new Account(User);    
            
            InitializeComponent();

            ApplySettings();
            SetWindowSettings();

            CreateNotificationIcon();
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

          //  Application.Current.Resources.MergedDictionaries.RemoveAt(Application.Current.Resources.MergedDictionaries.Count - 1);
           
            ResourceDictionary mainDict = new ResourceDictionary { Source = new Uri($"MainWindow/Themes/{theme}Theme.xaml", UriKind.RelativeOrAbsolute) };
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

        private void SetWindowSettings()
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            WindowState = WindowState.Normal;
            Home.IsChecked = true;
        }

        private void CreateNotificationIcon()
        {
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("../../MainWindow/Assets/TrayIcon.ico");
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    Show();
                    WindowState = WindowState.Normal;
                };
        }

        private void MaximizeWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            if (WindowState == WindowState.Maximized && MaximizeWindow.IsChecked == false)
                MaximizeWindow.IsChecked = true;
            if (WindowState == WindowState.Normal && MaximizeWindow.IsChecked == true)
                MaximizeWindow.IsChecked = false;

            base.OnStateChanged(e);
        }
        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            Animation.NavMenuAnimation(Axis, 0);
            MainFrame.Content = HomePage;
        }

        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            Animation.NavMenuAnimation(Axis, 504);
            MainFrame.Content = SettingsPage;
        }

        private void Account_Checked(object sender, RoutedEventArgs e)
        {
            Animation.NavMenuAnimation(Axis, 1008);           
            MainFrame.Content = AccountPage;
        }

        
        public void ShowNotification(string NotificationMessage)
        {
            Animation.ShowNotificationAnimation(NotificationBody);
            NotificationText.Text = NotificationMessage;
        }


        private void MaximizeWindow_Checked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
