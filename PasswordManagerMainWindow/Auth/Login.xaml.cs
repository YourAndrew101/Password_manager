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

namespace PasswordManager.Auth
{
    public partial class Login : Page
    {
        private User _user;

        private string Email { get => LoginTextBox.Text; }
        private string Password { get => PasswordTextBox.Password; }


        public Login()
        {
            InitializeComponent();

            SetConnectionDataBase();
        }

        private void SetConnectionDataBase()
        {
            User.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuthData()) return;

            try { _user = User.GetUser(Email, Password); }
            catch (Exception ex)
            {
                if (ex is NonExistenMailException nonExistenMail) { SetErrorMessage(nonExistenMail.Message); return; }
                if (ex is IncorrectPasswordException) { SetErrorMessage("Incorrect password"); return; }

                throw;
            }

            MainWindow.MainWindow mainWindow = new MainWindow.MainWindow(_user);
            mainWindow.Show();
            var parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private bool CheckAuthData()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email)) { SetErrorMessage("Enter Email"); return false; }
            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password)) { SetErrorMessage("Enter password"); return false; }
            try { _ = new MailAddress(Email).Address; }
            catch (FormatException) { SetErrorMessage("Enter valid Email"); return false; }

            return true;
        }

        private void SetErrorMessage(string message)
        {
            AurhError.Visibility = Visibility.Visible;
            AurhError.Text = message;
        }
    }
}
