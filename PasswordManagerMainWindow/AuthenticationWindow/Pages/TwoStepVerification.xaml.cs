using System.Windows;
using System.Windows.Controls;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary;
using UsersLibrary.Settings;

namespace PasswordManager.AuthenticationWindow.Pages
{
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

            if (_eMailService.ConfirmationCode == CurrentConfirmationCode)
            {
                if (_saveSettingsFlag)
                {
                    ISettingsService settingsService = new SignUpSettingsService();
                    settingsService.SaveSettings((SignUpSettings)_user);                
                }

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