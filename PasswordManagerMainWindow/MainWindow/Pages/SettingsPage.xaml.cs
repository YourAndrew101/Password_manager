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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            // TrayTB.IsChecked = true;


        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            //    ControlTemplate template = TrayTB.Template;
            //    Border frontRect = TrayTB.Template.FindName("frontRect", TrayTB) as Border;
            //    DoubleAnimation movement = new DoubleAnimation
            //    {
            //        Duration = TimeSpan.FromSeconds(0.3),
            //        From =10,
            //        To = -10,
            //    };
            //    frontRect.RenderTransform.BeginAnimation(TranslateTransform.XProperty, movement);
            //    DoubleAnimation widthAnimation1 = new DoubleAnimation()
            //    {
            //        From = 20,
            //        To = 35,
            //        Duration = TimeSpan.FromSeconds(0.15),
            //    };
            //    frontRect.BeginAnimation(WidthProperty, widthAnimation1);
            //    DoubleAnimation widthAnimation2 = new DoubleAnimation()
            //    {
            //        From = 35,
            //        To = 20,
            //        Duration = TimeSpan.FromSeconds(0.15),
            //        BeginTime = TimeSpan.FromSeconds(0.15),
            //    };
            //    frontRect.BeginAnimation(WidthProperty, widthAnimation2);
        }

        private void PasswordLengthSettingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text;

            if (text.Any())
                if (!text.All(c => char.IsDigit(c)))
                {
                    int temp = text.IndexOf(text.First(c => !char.IsDigit(c)));
                    (sender as TextBox).Text = text.Remove(temp, 1);
                    (sender as TextBox).CaretIndex = text.Length;
                }
        }
    }
}
