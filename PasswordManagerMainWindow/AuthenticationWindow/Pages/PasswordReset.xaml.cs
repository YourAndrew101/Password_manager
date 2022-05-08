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
using ServicesLibrary;

namespace PasswordManager.AuthenticationWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для PasswordReset.xaml
    /// </summary>
    public partial class PasswordReset : Page
    {
        public PasswordReset()
        {
            InitializeComponent();
        }

        private void SendCode_Click(object sender, RoutedEventArgs e)
        {
            try { _ = new MailAddress(EmailTextBox.Text).Address; }
            catch (FormatException) { SetErrorMessage(Properties.Resources.EnterValidEmail); }

            if (UsersService.IsExistsEmail(EmailTextBox.Text))
            {
                SendCode.IsEnabled = false;
                EmailTextBox.IsEnabled = false;
                ErrorTextBlock.Text = "";
            }
            else
            {
                SetErrorMessage(Properties.Resources.UserNotExists);
            }
        }
        private void SetErrorMessage(string message)
        {
            ErrorTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            ErrorTextBlock.Text = message;
        }
    }
}
