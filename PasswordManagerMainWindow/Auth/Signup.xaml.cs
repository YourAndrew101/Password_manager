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
using Services;
using UsersLibrary;
using UsersLibrary.Settings;
using static UsersLibrary.UsersExceptions;

namespace PasswordManager.Auth
{
    /// <summary>
    /// Логика взаимодействия для Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private string Password { get => KindaPasswordTextBox.Text; }
        private string Email { get => EmailTextBox.Text; }

        private string PasswordComplexityText { set => PasswordComplexityTextBlock.Text = value; }
        private Brush PasswordComplexityTextColor { set => PasswordComplexityTextBlock.Foreground = value; }

        private readonly Rectangle[] _passwordComplexityRectangles = new Rectangle[5];

        private Color _veryWeakPasswordRectangleColor = (Color)Application.Current.Resources["_veryWeakPasswordRectangleColor"];
        private Color _weakPasswordRectangleColor = (Color)Application.Current.Resources["_weakPasswordRectangleColor"];
        private Color _normalPasswordRectangleColor = (Color)Application.Current.Resources["_normalPasswordRectangleColor"];
        private Color _strongPasswordRectangleColor = (Color)Application.Current.Resources["_strongPasswordRectangleColor"];
        private Color _veryStrongPasswordRectangleColor = (Color)Application.Current.Resources["_veryStrongPasswordRectangleColor"];
        private Color _nullPasswordRectangleColor = (Color)Application.Current.Resources["PasswordCompexityRectangle.Static.Fill"];

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


        private enum PasswordScore
        {
            Null = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }
        private static PasswordScore CheckStrength(string password)
        {
            int passwordComplexity = 1;

            if (password.Length < 4) return PasswordScore.Null;
            if (password.Length < 8) return PasswordScore.VeryWeak;

            if (password.Length >= 8) passwordComplexity++;
            if (password.Length >= 12) passwordComplexity++;
            if (password.Length >= 16) passwordComplexity++;
            if (password.Any(char.IsDigit)) passwordComplexity++;
            if (password.Any(char.IsSymbol)) passwordComplexity++;
            if (password.Length - password.Distinct().Count() >= password.Length / 3) passwordComplexity--;

            return (PasswordScore)passwordComplexity;
        }


        private void KindaPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetPasswordComplexity(CheckStrength(Password));
            SetShowPasswordToggleButton();

            if (AuthErrorTextBlock.Visibility == Visibility.Visible) HideErrorMessage();
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

        private void SetPasswordComplexity(PasswordScore passwordScore)
        {
            switch (passwordScore)
            {
                case PasswordScore.VeryWeak:
                    SolidColorBrush colorBrush = new SolidColorBrush(_veryWeakPasswordRectangleColor);
                    SetPasswordComplexityText(Properties.Resources.PswdVeryWeak, colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.Weak:
                    colorBrush = new SolidColorBrush(_weakPasswordRectangleColor);
                    SetPasswordComplexityText(Properties.Resources.PswdWeak, colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.Medium:
                    colorBrush = new SolidColorBrush(_normalPasswordRectangleColor);
                    SetPasswordComplexityText(Properties.Resources.PswdNormal, colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.Strong:
                    colorBrush = new SolidColorBrush(_strongPasswordRectangleColor);
                    SetPasswordComplexityText(Properties.Resources.PswdStrong, colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.VeryStrong:
                    colorBrush = new SolidColorBrush(_veryStrongPasswordRectangleColor);
                    SetPasswordComplexityText(Properties.Resources.PswdVeryStrong, colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                default:
                    colorBrush = new SolidColorBrush(_nullPasswordRectangleColor);
                    SetPasswordComplexityText("", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
            }
        }
        private void SetPasswordComplexityText(string message, SolidColorBrush colorBrush)
        {
            PasswordComplexityText = message;
            PasswordComplexityTextColor = colorBrush;
        }

        private void SetPasswordComplexityRectangles(SolidColorBrush colorBrush, PasswordScore passwordScore)
        {
            ClearPasswordComplexityRectangles();

            for (int i = 0; i < (int)passwordScore; i++)
                _passwordComplexityRectangles[i].Fill = colorBrush;
        }
        private void ClearPasswordComplexityRectangles()
        {
            foreach (var item in _passwordComplexityRectangles)
                item.Fill = new SolidColorBrush(_nullPasswordRectangleColor);
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
                    SetErrorMessage(Properties.Resources.UserExists);
                    return;
                }
                throw;
            }

            SettingsService.SaveSignUpSettings(new Settings(user));

            EmailConfirmation nextPage = new EmailConfirmation(user);
            NavigationService navService = NavigationService.GetNavigationService(this);
            navService.Navigate(nextPage);
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
        private void HideErrorMessage()
        {
            AuthErrorTextBlock.Visibility = Visibility.Hidden;
        }
    }
}