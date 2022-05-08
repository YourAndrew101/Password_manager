using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Services;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.AuthenticationWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmailConfirmation.xaml
    /// </summary>
    public partial class EmailConfirmation : Page
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

        EMailService _eMailService;
        User _user;

        public EmailConfirmation(User user)
        {
            _user = user;

            InitializeComponent();
            SendEmail();
        }

        private void SendEmail()
        {
            _eMailService = new EMailService(_user.Email);
            _eMailService.SendConfirmationMessage();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(_eMailService.ConfirmationCode == CurrentConfirmationCode)
            {
                //UsersService.AddUser(_user);

                MainWindow.MainWindow mainWindow = new MainWindow.MainWindow(_user);
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
            else
            {
                //TODO обмежена кількість спроб
                ErrorMessage = Properties.Resources.PasswordResetWrongCode;
            }
        }
    }
}