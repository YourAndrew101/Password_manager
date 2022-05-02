using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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



        }



        private void deletesomeshit_Click(object sender, RoutedEventArgs e)
        {
            dataVM.AuthenticationData.RemoveAt(1);
        }

        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditForm.Visibility = Visibility.Visible;
            ShadowEffectHomePage.Visibility = Visibility.Visible;
            MainWindow.ShadowEffectWindow.Visibility = Visibility.Visible;
            DoubleAnimation formScaleAnimation = new DoubleAnimation()
            {
                From = 1.2,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                AccelerationRatio = 0.5,
            };
            AddEditForm.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, formScaleAnimation);
            AddEditForm.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, formScaleAnimation);

            DoubleAnimation formOpacityAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.1),

            };
            AddEditForm.BeginAnimation(OpacityProperty, formOpacityAnimation);

            DoubleAnimation shadowAppearingAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 0,
                To = 0.75,
            };
            ShadowEffectHomePage.BeginAnimation(OpacityProperty, shadowAppearingAnimation);
            MainWindow.ShadowEffectWindow.BeginAnimation(OpacityProperty, shadowAppearingAnimation);


        }

        private void CloseEditAddForm_Click(object sender, RoutedEventArgs e)
        {
            FormClosingAnimation();

        }
        private void FormClosingAnimation()
        {
            DoubleAnimation formScaleAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(0.2),
                AccelerationRatio = 0.5,
            };
            formScaleAnimation.Completed += formDisapperingAnimation_Comleted;
            AddEditForm.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, formScaleAnimation);
            AddEditForm.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, formScaleAnimation);

            DoubleAnimation formOpacityAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.1),


            };
            AddEditForm.BeginAnimation(OpacityProperty, formOpacityAnimation);

            DoubleAnimation shadowDisappearingAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 0.75,
                To = 0,
            };
            shadowDisappearingAnimation.Completed += ShadowDisappearingAnimation_Completed;
            ShadowEffectHomePage.BeginAnimation(OpacityProperty, shadowDisappearingAnimation);
            MainWindow.ShadowEffectWindow.BeginAnimation(OpacityProperty, shadowDisappearingAnimation);
        }
        private void ShadowDisappearingAnimation_Completed(object sender, EventArgs e)
        {
            ShadowEffectHomePage.Visibility = Visibility.Collapsed;
            MainWindow.ShadowEffectWindow.Visibility = Visibility.Collapsed;
        }

        private void formDisapperingAnimation_Comleted(object sender, EventArgs e)
        {
            AddEditForm.Visibility = Visibility.Collapsed;

        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SuggestPassword.Visibility = Visibility.Visible;
            DoubleAnimation suggestionApperingAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
            };

            SuggestPassword.BeginAnimation(OpacityProperty, suggestionApperingAnimation);
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation suggestionDisapperingAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            suggestionDisapperingAnimation.Completed += SuggestionDisappeared;
            SuggestPassword.BeginAnimation(OpacityProperty, suggestionDisapperingAnimation);

        }

        private void SuggestionDisappeared(object sender, EventArgs e)
        {
            SuggestPassword.Visibility = Visibility.Collapsed;
        }

        private void SuggestPassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = "test password";
        }

        private void FormSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            dataVM.AuthenticationData.Add(new Models.AuthenticationData(ResourceTextBox.Text, LoginTextBox.Text, PasswordTextBox.Text));
            FormClosingAnimation();
            ShowNotification("Record was added");
        }

        private void ShowNotification(string NotificationMessage)
        {
            NotificationBody.Visibility = Visibility.Visible;

            NotificationText.Text = NotificationMessage;

            DoubleAnimation notificationAppearingTranslate = new DoubleAnimation()
            {
                From = 0,
                To = -30,
                Duration = TimeSpan.FromSeconds(0.3),
                AccelerationRatio = 0.5

            };
            DoubleAnimation notificationAppearingOpacity = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
            };
            DoubleAnimation notificationDisappearingTranslate = new DoubleAnimation()
            {
                From = -30,
                To = 0,
                BeginTime = TimeSpan.FromSeconds(2),
                Duration = TimeSpan.FromSeconds(0.3),
                AccelerationRatio = 0.5
            };
            DoubleAnimation notificationDisappearingOpacity = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                BeginTime = TimeSpan.FromSeconds(2),
            };
            Storyboard.SetTargetName(notificationAppearingTranslate, "NotificationTranslateTransform");
            Storyboard.SetTargetProperty(notificationAppearingTranslate, new PropertyPath(TranslateTransform.YProperty));
            var sb = new Storyboard();
            sb.Completed += Notification_Completed;
            sb.Children.Add(notificationAppearingTranslate);

            Storyboard.SetTarget(notificationAppearingOpacity, NotificationBody);
            Storyboard.SetTargetProperty(notificationAppearingOpacity, new PropertyPath(Border.OpacityProperty));
            sb.Children.Add(notificationAppearingOpacity);

            Storyboard.SetTargetName(notificationDisappearingTranslate, "NotificationTranslateTransform");
            Storyboard.SetTargetProperty(notificationDisappearingTranslate, new PropertyPath(TranslateTransform.YProperty));
            sb.Children.Add(notificationDisappearingTranslate);

            Storyboard.SetTarget(notificationDisappearingOpacity, NotificationBody);
            Storyboard.SetTargetProperty(notificationDisappearingOpacity, new PropertyPath(Border.OpacityProperty));
            sb.Children.Add(notificationDisappearingOpacity);

            sb.Begin(NotificationBody);


        }

        private void Notification_Completed(object sender, EventArgs e)
        {
            NotificationBody.Visibility = Visibility.Collapsed;
        }
    }
}
