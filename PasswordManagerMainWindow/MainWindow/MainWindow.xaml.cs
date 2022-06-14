using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using PasswordManager.MainWindow.Pages;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.MainWindow
{
    public partial class MainWindow : Window
    {  
        private User User { get; set; }
        public Home HomePage { get; set; }
        public Settings SettingsPage { get; set; }
        public Account AccountPage { get; set; }

        private bool ToTray 
        {
            get 
            {
                ISettingsService settingsService = new WindowSettingsService();
                return ((WindowSettings)settingsService.GetSettings()).ToTray;
            }
        }

        private System.Windows.Forms.NotifyIcon _notifyIcon;

        private delegate void ActionClose();
        private ActionClose _actionClose;

        
        public MainWindow(User user)
        {
            User = user;
            HomePage = new Home(User);
            SettingsPage = new Settings(User);
            AccountPage = new Account(User);

            StateChanged += MainWindow_StateChanged;
            
            InitializeComponent();
            
            ApplySettings();
            SetWindowSettings();
           
            CreateNotificationIcon();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized && MaximizeWindow.IsChecked == false)
                MaximizeWindow.IsChecked = true;
            if (WindowState == WindowState.Normal && MaximizeWindow.IsChecked == true)
                MaximizeWindow.IsChecked = false;
        }

        public void ApplySettings()
        {
            ISettingsService settingsService = new WindowSettingsService();

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

        private void SetWindowSettings()
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            WindowState = WindowState.Normal;
            Home.IsChecked = true;
        }

        private void CreateNotificationIcon()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("../../MainWindow/Assets/TrayIcon.ico"),
                Visible = true
            };
            _notifyIcon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    Show();
                    WindowState = WindowState.Normal;
                };
        }
        public void DeleteNotificationIcon()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
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


        private void MaximizeWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
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
            if (ToTray) _actionClose = CloseToTrey;
            else _actionClose = CloseWithExit;

            _actionClose();
        }
        private void CloseToTrey()
        {
            Hide();
            base.OnStateChanged(new EventArgs());
        }
        private void CloseWithExit()
        {
            DeleteNotificationIcon();
            Close();
        }       
    }
}
