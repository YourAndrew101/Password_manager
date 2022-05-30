﻿using System;
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
        }

        private void DisplaySettings()
        {
            if (!settingsService.IsSavedSettings) return;

            WindowSettings settings = (WindowSettings)settingsService.GetSettings();
            //WindowSettings settings = new WindowSettings(WindowSettings.Languages.English, WindowSettings.Themes.Light, true, 16);

            LanguageSelector.SelectedIndex = (int)settings.Language;
            ThemeSelector.SelectedIndex = (int)settings.Theme;
            
            TrayToggleButton.IsChecked = settings.ToTray;
            
            PasswordLengthSettingTextBox.Text = settings.PasswordGenerateLength.ToString();
        }

        private void PasswordLengthSettingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text;

            if (text.Any())
                if (!text.All(c => char.IsDigit(c)))
                {
                    int temp = text.IndexOf(text.First(c => !char.IsDigit(c)));
                    (sender as TextBox).Text = text.Remove(temp, 1);
                    (sender as TextBox).CaretIndex = text.Length;
                }
        }

        private void SaveSettings(object sender, SelectionChangedEventArgs e)
        {
            //WindowSettings.Languages language = GetLanguageSetting();
          //  WindowSettings.Themes theme = GetThemeSetting();
           // bool toTray = GetToTraySetting();
            //int passwordGenerateLength = GetPasswordGenerateLengthSetting();

          //  WindowSettings settings = new WindowSettings(language, theme, toTray, passwordGenerateLength);
            //settingsService.Save(settings);
        }
        private WindowSettings.Languages GetLanguageSetting()
        {
            throw new NotImplementedException();
        }
        private WindowSettings.Themes GetThemeSetting()
        {
            throw new NotImplementedException();
        }
        private bool GetToTraySetting()
        {
            throw new NotImplementedException();
        }
        private int GetPasswordGenerateLengthSetting()
        {
            throw new NotImplementedException();
        }
    }
}
