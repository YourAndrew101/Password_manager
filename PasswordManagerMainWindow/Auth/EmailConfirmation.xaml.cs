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
using UsersLibrary.Settings;

namespace PasswordManager.Auth
{
    /// <summary>
    /// Логика взаимодействия для EmailConfirmation.xaml
    /// </summary>
    public partial class EmailConfirmation : Page
    {
        private string _email;

        public EmailConfirmation()
        {
            InitializeComponent();
            GetDataFromSettings();
            SendEmail();
        }

        private void GetDataFromSettings() => _email = SettingsService.GetSignUpSettings().Email;

        private async Task SendEmail()
        {
            EMailService eMailService = new EMailService(_email);
            await eMailService.SendConfirmationMessage();
            string a = eMailService.ConfirmationCode;

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}