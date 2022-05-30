using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServicesLibrary.SettingsService;
using UsersLibrary.Settings;

namespace PasswordManager.MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private readonly ISettingsService settingsService;

        public Settings()
        {
            settingsService = new WindowSettingsService();

            InitializeComponent();
            DisplaySettings();
            SubscribeToEvents();
        }

        private void DisplaySettings()
        {
            if (!settingsService.IsSavedSettings) return;

            WindowSettings settings = (WindowSettings)settingsService.GetSettings();

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

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveSettings();
        }

        private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

            WindowSettings settings = new WindowSettings(language, theme, toTray, passwordGenerateLength);
            settingsService.Save(settings);
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
