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
using PasswordManager.MainWindow;
using PasswordManagerWindow.ViewModels;

namespace PasswordManagerWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page

    {
        AuthenticationDataVM dataVM = new AuthenticationDataVM();
        public Home()
        {
            InitializeComponent();
            DataContext = dataVM;
            var w = App.Current.MainWindow;
           
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(SearchTextBox.Text==Properties.Resources.PlaceholderSearch)
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = (SolidColorBrush)TryFindResource("TextColor");
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text)) {
                SearchTextBox.Text = Properties.Resources.PlaceholderSearch;
                SearchTextBox.Foreground = (SolidColorBrush)TryFindResource("SearchTextBoxStyle.Placeholder.Foreground");
            }
        }

        private void deletesomeshit_Click(object sender, RoutedEventArgs e)
        {
            dataVM.AuthenticationData.RemoveAt(1);
        }

        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddEditForm.Visibility = Visibility.Visible;
           MainWindow.ShadowEffect.Visibility = Visibility.Visible;
            
        }
    }
}
