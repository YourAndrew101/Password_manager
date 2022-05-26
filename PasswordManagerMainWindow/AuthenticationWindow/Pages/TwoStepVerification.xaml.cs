﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.AuthenticationWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для TwoStepVerification.xaml
    /// </summary>
    public partial class TwoStepVerification : Page
    {
        private string CurrentConfirmationCode { get => CurrentConfirmationCodeTextBox.Text; }
        private string ErrorMessage
        {
            set
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
                ErrorTextBlock.Text = value;
            }
        }

        private EMailService _eMailService;
        private readonly User _user;
        private readonly bool _saveSettingsFlag;

        public TwoStepVerification(User user, bool saveSettingsFlag)
        {
            _user = user;
            _saveSettingsFlag = saveSettingsFlag;

            InitializeComponent();          
            SendEmail();
        }

        private async void SendEmail()
        {
            _eMailService = new EMailService(_user.Email);
            await _eMailService.SendAsyncConfirmationMessage();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            ISettingsService settingsService = new SignUpSettingsService();
            if (_eMailService.ConfirmationCode == CurrentConfirmationCode)
            {
                if(_saveSettingsFlag) settingsService.Save((SignUpSettings)_user);

                ((AuthenticationWindow)Window.GetWindow(this)).StartMainWindow(_user);
            }
            else
            {
                //TODO обмежена кількість спроб
                ErrorMessage = Properties.Resources.PasswordResetWrongCode;
            }
        }

        private void CurrentConfirmationCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorTextBlock.Visibility = Visibility.Hidden;
        }
    }
}