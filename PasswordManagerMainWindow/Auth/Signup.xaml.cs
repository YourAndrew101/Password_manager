using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PasswordManager.Auth
{
    /// <summary>
    /// Логика взаимодействия для Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private string Password { get => PasswordTextBox.Password; }
        private string PasswordComplexityText { set => PasswordComplexityTextBlock.Text = value; }
        private Brush PasswordComplexityTextColor { set => PasswordComplexityTextBlock.Foreground = value; }

        private Rectangle[] _passwordComplexityRectangles = new Rectangle[5];

        private Color _veryWeakPasswordRectangleColor = (Color)ColorConverter.ConvertFromString("#FF0000");
        private Color _weakPasswordRectangleColor = (Color)ColorConverter.ConvertFromString("#DB7A19");
        private Color _normalPasswordRectangleColor = (Color)ColorConverter.ConvertFromString("#F1DD1A");
        private Color _strongPasswordRectangleColor = (Color)ColorConverter.ConvertFromString("#9CF11A");
        private Color _veryStrongPasswordRectangleColor = (Color)ColorConverter.ConvertFromString("#05C612");
        private Color _nullPasswordRectangleColor = (Color)ColorConverter.ConvertFromString("#EBEBEB");

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
                    SetPasswordComplexitText("Password very weak", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.Weak:
                    colorBrush = new SolidColorBrush(_weakPasswordRectangleColor);
                    SetPasswordComplexitText("Password weak", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.Medium:
                    colorBrush = new SolidColorBrush(_normalPasswordRectangleColor);
                    SetPasswordComplexitText("Password normal", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.Strong:
                    colorBrush = new SolidColorBrush(_strongPasswordRectangleColor);
                    SetPasswordComplexitText("Password strong", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                case PasswordScore.VeryStrong:
                    colorBrush = new SolidColorBrush(_veryStrongPasswordRectangleColor);
                    SetPasswordComplexitText("Password very strong", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
                default:
                    colorBrush = new SolidColorBrush(_nullPasswordRectangleColor);
                    SetPasswordComplexitText("", colorBrush);
                    SetPasswordComplexityRectangles(colorBrush, passwordScore);
                    break;
            }

        }

        private void SetPasswordComplexitText(string message, SolidColorBrush colorBrush)
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
            KindaPassword.Visibility = Visibility.Visible;
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            KindaPassword.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
        }
    }
}