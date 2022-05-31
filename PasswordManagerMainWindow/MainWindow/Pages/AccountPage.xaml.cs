using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServicesLibrary.SettingsService;
using UsersLibrary.Settings;

namespace PasswordManager.MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : Page
    {

        private SignUpSettingsService _service;
        private SignUpSettings _settings;
        public Account()
        {
            InitializeComponent();
            _service = new SignUpSettingsService();
            _settings = (SignUpSettings)_service.GetSettings();
            EmailTextBlock.Text = _settings.Email;

        }
    }
}
