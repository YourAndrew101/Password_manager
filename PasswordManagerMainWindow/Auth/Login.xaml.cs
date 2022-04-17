using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using UsersLibrary;
using PasswordManager.MainWindow;
using System.Configuration;
using static UsersLibrary.UsersExceptions;
using Services;

namespace PasswordManager.Auth
{
    public partial class Login : Page
    {
        private User _user;

        private string Email { get => EmailTextBox.Text; } 
        private string Password { get => PasswordTextBox.Password; }


        public Login()
        {
            InitializeComponent();

            SetConnectionDataBase();
        }
        private void SetConnectionDataBase()
        {
            UsersService.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuthData()) return;

            //TODO обмежена кількість спроб
            try { _user = UsersService.GetUser(Email, Password); }
            catch (Exception ex)
            {
                if (ex is NonExistenMailException nonExistenMail) { SetErrorMessage(nonExistenMail.Message); return; }
                if (ex is IncorrectPasswordException) { SetErrorMessage(Properties.Resources.IncorrectPassword); return; }

                throw;
            }

            MainWindow.MainWindow mainWindow = new MainWindow.MainWindow(_user);
            mainWindow.Show();
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private bool CheckAuthData()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email)) { SetErrorMessage(Properties.Resources.EmailRequest); return false; }
            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password)) { SetErrorMessage(Properties.Resources.PasswordRequest); return false; }
            try { _ = new MailAddress(Email).Address; }
            catch (FormatException) { SetErrorMessage(Properties.Resources.EnterValidEmail); return false; }

            return true;
        }
        //TODO функціонал запам'ятати мене
        private void SetErrorMessage(string message)
        {
            AuthErrorTextBlock.Visibility = Visibility.Visible;
            AuthErrorTextBlock.Text = message;
        }


        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Visibility = Visibility.Collapsed;
            KindaPasswordTextBox.Visibility = Visibility.Visible;
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            KindaPasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
        }

        private void KindaPassword_TextChanged(object sender, TextChangedEventArgs e)
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