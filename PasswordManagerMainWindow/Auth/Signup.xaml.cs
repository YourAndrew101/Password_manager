using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PasswordManager.Auth
{
    /// <summary>
    /// Логика взаимодействия для Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        string Password;
        public Signup()
        {
            InitializeComponent();
            
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
          NavigationService.Navigate(new EmailConfirmation(LoginTextBox.Text,FakePassword.Text));
        }

        private void FakePassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Password+=e.Text;
            //e.Text.Replace(e.Text.Substring(e.Text.Length - 1), '@');
            //FakePassword.Text += "@";
        }

        private void FakePassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            FakePassword.Text=FakePassword.Text.Replace(FakePassword.Text.ToCharArray()[FakePassword.Text.Length-1],'@');
            FakePassword.CaretIndex++;
        }
    }
}
