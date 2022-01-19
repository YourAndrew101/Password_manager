using System;
using System.Collections.Generic;
using System.Configuration;
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
using UsersLibrary;

namespace PasswordManagerMainWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;

            User.ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

            try {
                if (User.CheckPassword(login, password)) MessageBox.Show("Success");
                else MessageBox.Show("Lox");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}