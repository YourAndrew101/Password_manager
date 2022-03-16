﻿using System;
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
using PasswordManager.Services;

namespace PasswordManager.Auth
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
            SetPage();
            SetColorTheme();

            CultureInfo.CurrentCulture = new CultureInfo("uk-UA", false);
        }

        private void SetPage()
        {
            AuthFrame.Content = new Login();
        }

        private void SetColorTheme()
        {
            ThemePicker.Themes theme = ThemePicker.GetSystemTheme();

            ResourceDictionary dict = new ResourceDictionary { Source = new Uri($"/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
