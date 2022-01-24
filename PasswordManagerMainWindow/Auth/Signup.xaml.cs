using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class Signup : Page, INotifyPropertyChanged
    {
        private string _passwordComplexityText;
        public string PasswordComplexityText {
            get { return _passwordComplexityText; }
            set
            {
                _passwordComplexityText = value;
                OnPropertyChanged("PasswordComplexityText");
            }
        }

        public Signup()
        {
            InitializeComponent();
            PasswordComplexityText = "test";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
