using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App() {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
           // ResourceDictionary dict = new ResourceDictionary();
           // dict.Source = new Uri("ColorTheme.xaml", UriKind.Relative);
           // Application.Current.Resources.MergedDictionaries.Add(dict);
           //Current.Resources.Remove("ColorTheme.xzml");
        }
    }
}
