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

namespace PasswordManager.Components
{
    public partial class BindablePasswordBox : UserControl
    {
       

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
            new PropertyMetadata(string.Empty, PasswordPropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox bindablePassword)
            {
                bindablePassword.UpdatePasswordBox();
            }
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }
        private bool UpdatePasswordBoxFlag;
        private void UpdatePasswordProperty(object sender, RoutedEventArgs e)
        {
            UpdatePasswordBoxFlag = true;
            Password = passwordBox.Password;
            UpdatePasswordBoxFlag = false;
        }
        private  void UpdatePasswordBox()
        {
            if (!UpdatePasswordBoxFlag)
            {
                passwordBox.Password = Password;
            }
        }
    }
}
