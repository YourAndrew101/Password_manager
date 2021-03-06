using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ServicesLibrary.SettingsService;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.MainWindow.Pages
{
    public partial class Settings : Page
    {
        private readonly ISettingsService _settingsService;
        private User _user;

        public Settings(User user)
        {
            _settingsService = new WindowSettingsService();
            _user = user;

            InitializeComponent();
            DisplaySettings();
            SubscribeToEvents();
        }

        private void DisplaySettings()
        {
            if (!_settingsService.IsSavedSettings) return;

            WindowSettings settings = (WindowSettings)_settingsService.GetSettings();

            LanguageSelector.SelectedIndex = (int)settings.Language;
            ThemeSelector.SelectedIndex = (int)settings.Theme;

            TrayToggleButton.IsChecked = settings.ToTray;
            PasswordGenerateLengthSettingTextBox.Text = settings.PasswordGenerateLength.ToString();
        }
        private void SubscribeToEvents()
        {
            LanguageSelector.SelectionChanged += LanguageSelector_SelectionChanged;
            ThemeSelector.SelectionChanged += ThemeSelector_SelectionChanged;
            TrayToggleButton.Click += TrayToggleButton_Click;
            PasswordGenerateLengthSettingTextBox.TextChanged += PasswordLengthSettingTextBox_TextChanged;
        }

        private void PasswordLengthSettingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text;

            if (text.Any())
            {
                if (!text.All(c => char.IsDigit(c)))
                {
                    int temp = text.IndexOf(text.First(c => !char.IsDigit(c)));
                    (sender as TextBox).Text = text.Remove(temp, 1);
                    (sender as TextBox).CaretIndex = text.Length;
                }

                SaveSettings();
            }
        }

        private void CreateNewPages()
        {
            ((App)Application.Current).GetMainWindow.ApplySettingsLanguage();
            ((App)Application.Current).GetMainWindow.HomePage = new Home(_user);
            ((App)Application.Current).GetMainWindow.SettingsPage = new Settings(_user);
            ((App)Application.Current).GetMainWindow.AccountPage = new Account(_user);
            ((App)Application.Current).GetMainWindow.MainFrame.Content = ((App)Application.Current).GetMainWindow.SettingsPage;
        }
        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveSettings();
            CreateNewPages();
        }
        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((App)Application.Current).GetMainWindow.ShowNotification(Properties.Resources.ThemeSettingsChange);
            SaveSettings();
        }

        private void TrayToggleButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            WindowSettings.Languages language = GetLanguageSetting();
            WindowSettings.Themes theme = GetThemeSetting();
            bool toTray = GetToTraySetting();
            int passwordGenerateLength = GetPasswordGenerateLengthSetting();

            ISettings settings = new WindowSettings(language, theme, toTray, passwordGenerateLength);
            _settingsService.SaveSettings(settings);
        }
        private WindowSettings.Languages GetLanguageSetting() => (WindowSettings.Languages)LanguageSelector.SelectedIndex;
        private WindowSettings.Themes GetThemeSetting() => (WindowSettings.Themes)ThemeSelector.SelectedIndex;
        private bool GetToTraySetting() => (bool)TrayToggleButton.IsChecked;
        private int GetPasswordGenerateLengthSetting()
        {
            if (string.IsNullOrEmpty(PasswordGenerateLengthSettingTextBox.Text))
                return WindowSettings.StandartPasswordGenerateLength;

            return Convert.ToInt32(PasswordGenerateLengthSettingTextBox.Text);
        }
    }
}
