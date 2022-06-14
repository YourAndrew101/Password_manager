using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        private User _user;

        private string Email
        {
            get => _user.Email;
        }
        private EMailService _eMailService;

        public Account(User user)
        {
            InitializeComponent();
            _user = user;

            
            SetAccountFields();
        }
        private void SetAccountFields()
        {
            EmailTextBlock.Text = Email;

        }

        private void ChangeEmailFormSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData()) return;

            SendEmail();
            ChangeEmailForm.Visibility = Visibility.Collapsed;
            ConfirmEmailChangeForm.Visibility = Visibility.Visible;

        }
        private bool CheckData()
        {
            if (string.IsNullOrEmpty(NewEmailTextBox.Text) || string.IsNullOrWhiteSpace(NewEmailTextBox.Text)) { SetErrorMessage(ChangeEmailFormErrorTextBlock, Properties.Resources.EmailRequest); return false; }
          
            try { _ = new MailAddress(NewEmailTextBox.Text).Address; }
            catch (FormatException) { SetErrorMessage(ChangeEmailFormErrorTextBlock,Properties.Resources.EnterValidEmail); return false; }

            return true;
        }

        private void SetErrorMessage(TextBlock textBlock,string message)
        {
            textBlock.Visibility = Visibility.Visible;
            textBlock.Text = message;
        }
       
        private void CloseForm(object sender, RoutedEventArgs e)
        {
            Grid form = (Grid)((Button)sender).Parent;
            Animation.FormClosingAnimation(form, ShadowEffectAccountPage);
            ClearForm();

        }

        private void ClearForm()
        {
            NewEmailTextBox.Text = string.Empty;
            ConfirmEmailChangeCode.Text = string.Empty;
            ConfirmEmailChangeFormErrorTextBlock.Text = string.Empty;
            ConfirmEmailChangeFormErrorTextBlock.Visibility = Visibility.Collapsed;
            ChangeEmailFormErrorTextBlock.Text= string.Empty;
            ChangeEmailFormErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ChangeEmailButton_Click(object sender, RoutedEventArgs e)
        {
            Animation.FormOpeningAnimation(ChangeEmailForm, ShadowEffectAccountPage);
        }
    //TODO: Add logic to update field in db etc.
        private void ConfirmEmailChangeFormSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (_eMailService.ConfirmationCode == ConfirmEmailChangeCode.Text)
            {
            //    _email = NewEmailTextBox.Text;
            //SetAccountFields();
            Animation.FormClosingAnimation(ConfirmEmailChangeForm, ShadowEffectAccountPage);
            ClearForm();
            }
            else
            {
                SetErrorMessage(ConfirmEmailChangeFormErrorTextBlock, Properties.Resources.ConfirmEmailChangeWrongCode);
               
            }
           
        }
        private async void SendEmail()
        {
            _eMailService = new EMailService(_user.Email);
            await _eMailService.SendAsyncConfirmationMessage();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).ExitUser(_user);
        }
    }
}
