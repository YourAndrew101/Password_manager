using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
using UsersLibrary;

using static UsersLibrary.UsersExceptions;

namespace PasswordManager.AuthenticationWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private string Password { get => RevealedPasswordTextBox.Text; }
        private string Email { get => EmailTextBox.Text; }

        private string PasswordComplexityText { set => PasswordComplexityTextBlock.Text = value; }
        private Brush PasswordComplexityTextColor { set => PasswordComplexityTextBlock.Foreground = value; }

        private string ErrorMessage
        {
            set
            {
                AuthErrorTextBlock.Visibility = Visibility.Visible;
                AuthErrorTextBlock.Text = value;
            }
        }
        private void HideErrorMessage() => AuthErrorTextBlock.Visibility = Visibility.Hidden;

        private readonly Rectangle[] _passwordComplexityRectangles = new Rectangle[5];

        private readonly Color _veryWeakPasswordRectangleColor = (Color)Application.Current.Resources["_veryWeakPasswordRectangleColor"];
        private readonly Color _weakPasswordRectangleColor = (Color)Application.Current.Resources["_weakPasswordRectangleColor"];
        private readonly Color _normalPasswordRectangleColor = (Color)Application.Current.Resources["_normalPasswordRectangleColor"];
        private readonly Color _strongPasswordRectangleColor = (Color)Application.Current.Resources["_strongPasswordRectangleColor"];
        private readonly Color _veryStrongPasswordRectangleColor = (Color)Application.Current.Resources["_veryStrongPasswordRectangleColor"];
        private readonly Color _nullPasswordRectangleColor = (Color)Application.Current.Resources["PasswordCompexityRectangle.Static.Fill"];

        public Signup()
        {
            InitializeComponent();
            InitializePasswordComplexityRectangles();
        }
        private void InitializePasswordComplexityRectangles()
        {
            _passwordComplexityRectangles[0] = VeryWeakPasswordRectangle;
            _passwordComplexityRectangles[1] = WeakPasswordRectangle;
            _passwordComplexityRectangles[2] = NormalPasswordRectangle;
            _passwordComplexityRectangles[3] = StrongPasswordRectangle;
            _passwordComplexityRectangles[4] = VeryStrongPasswordRectangle;
        }

        //TODO: Rewirte toglebuttonShow for key preview
        private void KindaPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetPasswordComplexity();
            SetShowPasswordToggleButton();

            if (AuthErrorTextBlock.Visibility == Visibility.Visible) HideErrorMessage();
        }
        private void SetPasswordComplexity()
        {
            PasswordsService.PasswordScore passwordScore = PasswordsService.GetPasswordStrength(Password);

            SolidColorBrush solidColorBrush;
            string message = "";
            switch (passwordScore)
            {
                case PasswordsService.PasswordScore.VeryWeak:
                    solidColorBrush = new SolidColorBrush(_veryWeakPasswordRectangleColor);
                    message = Properties.Resources.PswdVeryWeak;
                    break;
                case PasswordsService.PasswordScore.Weak:
                    solidColorBrush = new SolidColorBrush(_weakPasswordRectangleColor);
                    message = Properties.Resources.PswdWeak;
                    break;
                case PasswordsService.PasswordScore.Medium:
                    solidColorBrush = new SolidColorBrush(_normalPasswordRectangleColor);
                    message = Properties.Resources.PswdNormal;
                    break;
                case PasswordsService.PasswordScore.Strong:
                    solidColorBrush = new SolidColorBrush(_strongPasswordRectangleColor);
                    message = Properties.Resources.PswdStrong;
                    break;
                case PasswordsService.PasswordScore.VeryStrong:
                    solidColorBrush = new SolidColorBrush(_veryStrongPasswordRectangleColor);
                    message = Properties.Resources.PswdVeryStrong;
                    break;
                default:
                    solidColorBrush = new SolidColorBrush(_nullPasswordRectangleColor);
                    break;
            }

            SetPasswordComplexityText(message, solidColorBrush);
            PasswordsService.SetPasswordComplexityRectangles(solidColorBrush, _nullPasswordRectangleColor, passwordScore, _passwordComplexityRectangles);
        }
        private void SetPasswordComplexityText(string message, SolidColorBrush colorBrush)
        {
            PasswordComplexityText = message;
            PasswordComplexityTextColor = colorBrush;
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AuthErrorTextBlock.Visibility == Visibility.Visible) HideErrorMessage();
        }

        private void SetShowPasswordToggleButton()
        {
            if (string.IsNullOrEmpty(Password)) ShowPasswordToggleButton.Visibility = Visibility.Hidden;
            else ShowPasswordToggleButton.Visibility = Visibility.Visible;
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

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuthData()) return;
            User user = new User(Email, Password);

            try
            {
                if (UsersService.IsExistsEmail(user.Email)) throw new DuplicateMailException();
            }
            catch (Exception ex)
            {
                if (ex is DuplicateMailException)
                {
                    ErrorMessage = Properties.Resources.UserExists;
                    return;
                }
                throw;
            }

            EmailConfirmation nextPage = new EmailConfirmation(user);
            NavigationService navService = NavigationService.GetNavigationService(this);
            navService.Navigate(nextPage);
        }
        private bool CheckAuthData()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Email)) { ErrorMessage = Properties.Resources.EmailRequest; return false; }
            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password)) { ErrorMessage = Properties.Resources.PasswordRequest; return false; }
            try { _ = new MailAddress(Email).Address; }
            catch (FormatException) { ErrorMessage = Properties.Resources.EnterValidEmail; return false; }

            return true;
        }        
    }
}