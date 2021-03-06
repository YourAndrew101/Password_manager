using System;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ServicesLibrary;
using UsersLibrary;
using static UsersLibrary.UsersExceptions;

namespace PasswordManager.AuthenticationWindow.Pages
{
    public partial class Login : Page
    {
        private User _user;

        private string Email { get => EmailTextBox.Text; }
        private string Password { get => HiddenPasswordTextBox.Password; }

        private bool? RememberMeFlag { get => RememberMeCheckBox.IsChecked; }

        public Login()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuthData()) return;

            //TODO обмежена кількість спроб
            try
            {
                _user = User.CreateAlreadyExistUser(Email, Password);
                _user = UsersService.GetHashAndSaltFromDB(_user);

               if (!UsersService.CheckUserData(_user)) return;
            }
            catch (Exception ex)
            {
                if (ex is NonExistenMailException) { SetErrorMessage(Properties.Resources.UserNotExists); return; }
                if (ex is IncorrectPasswordException) { SetErrorMessage(Properties.Resources.IncorrectPassword); return; }

                throw;
            }

            TwoStepVerification twoStepVerification = new TwoStepVerification(_user, (bool)RememberMeFlag);
            NavigationService.Navigate(twoStepVerification);
        }

        private bool CheckAuthData()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email)) { SetErrorMessage(Properties.Resources.EmailRequest); return false; }
            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password)) { SetErrorMessage(Properties.Resources.PasswordRequest); return false; }
            try { _ = new MailAddress(Email).Address; }
            catch (FormatException) { SetErrorMessage(Properties.Resources.EnterValidEmail); return false; }

            return true;
        }

        private void SetErrorMessage(string message)
        {
            AuthErrorTextBlock.Visibility = Visibility.Visible;
            AuthErrorTextBlock.Text = message;
        }


        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            HiddenPasswordTextBox.Visibility = Visibility.Collapsed;
            RevealedPasswordTextBox.Visibility = Visibility.Visible;

        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RevealedPasswordTextBox.Visibility = Visibility.Collapsed;
            HiddenPasswordTextBox.Visibility = Visibility.Visible;
        }

        private void RevealedPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetShowPasswordToggleButton();
        }

        private void SetShowPasswordToggleButton()
        {
            if (string.IsNullOrEmpty(Password)) ShowPasswordToggleButton.Visibility = Visibility.Hidden;
            else ShowPasswordToggleButton.Visibility = Visibility.Visible;
        }
    }
}