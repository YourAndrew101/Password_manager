using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ServicesLibrary;
using ServicesLibrary.SettingsService;
using UsersLibrary.Settings;

namespace PasswordManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString { get => ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString; }

        App()
        {
            DBConnectionSingleton.ConnectionString = ConnectionString;
        }

       
    }
}
