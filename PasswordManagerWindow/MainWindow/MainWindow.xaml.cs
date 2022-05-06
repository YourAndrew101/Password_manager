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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PasswordManager.Services;
using PasswordManagerWindow.Pages;
using UsersLibrary;

namespace PasswordManager.MainWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
           
        public MainWindow()
        {
            InitializeComponent();
            SetColorTheme();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            Home.IsChecked = true;
           

        }

        public MainWindow(User user)
        {
            InitializeComponent();
            SetColorTheme();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void SetColorTheme()
        {
            ThemesService.Themes theme = ThemesService.GetSystemTheme();

            ResourceDictionary dict = new ResourceDictionary { Source = new Uri($"/Themes/{theme}Theme.xaml", UriKind.Relative) };
            Application.Current.Resources.MergedDictionaries.Add(dict);

        }
        

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeWindow_Checked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
       

        private void MaximizeWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }
        //TODO: Rewrite this ffs
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if(this.WindowState==WindowState.Maximized && MaximizeWindow.IsChecked == false)
            {
                MaximizeWindow.IsChecked = true;
            }
            if(this.WindowState == WindowState.Normal && MaximizeWindow.IsChecked == true)
            {
                MaximizeWindow.IsChecked = false;
            }
        }
        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
            Axis.BeginAnimation(TranslateTransform.XProperty, da);
            MainFrame.Content = new Home();
        }

        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation(504, new Duration(TimeSpan.FromSeconds(0.3)));
            Axis.BeginAnimation(TranslateTransform.XProperty, da);
            MainFrame.Content = new Settings();
        }

        private void Account_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation(1008, new Duration(TimeSpan.FromSeconds(0.3)));
            Axis.BeginAnimation(TranslateTransform.XProperty, da);
            MainFrame.Content = new Account();
        }

     

        
    }
}
