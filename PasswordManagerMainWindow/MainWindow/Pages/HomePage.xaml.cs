﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data;
using Data.DataProviders.Products;
using Data.Repositories;
using PasswordManager.MainWindow;
using PasswordManager.MainWindow.ViewModels;
using ServicesLibrary;
using UsersLibrary;
using UsersLibrary.Services;

namespace PasswordManager.MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private ServiceRepository Repository { get; set; }

        private int _lengthGeneratePassword = 16;

        private User User { get; set; }

        AuthenticationDataVM dataVM;

        private string Password { get => HiddenPasswordTextBox.Text; }

        private string PasswordComplexityText { set => PasswordComplexityTextBlock.Text = value; }
        private Brush PasswordComplexityTextColor { set => PasswordComplexityTextBlock.Foreground = value; }

        private readonly Rectangle[] _passwordComplexityRectangles = new Rectangle[5];

        private readonly Color _veryWeakPasswordRectangleColor = (Color)Application.Current.Resources["_veryWeakPasswordRectangleColor"];
        private readonly Color _weakPasswordRectangleColor = (Color)Application.Current.Resources["_weakPasswordRectangleColor"];
        private readonly Color _normalPasswordRectangleColor = (Color)Application.Current.Resources["_normalPasswordRectangleColor"];
        private readonly Color _strongPasswordRectangleColor = (Color)Application.Current.Resources["_strongPasswordRectangleColor"];
        private readonly Color _veryStrongPasswordRectangleColor = (Color)Application.Current.Resources["_veryStrongPasswordRectangleColor"];
        private readonly Color _nullPasswordRectangleColor = (Color)Application.Current.Resources["PasswordCompexityRectangle.Static.Fill"];


        public Home(User user)
        {
            InitializeComponent();
            InitializePasswordComplexityRectangles();

            User = user;
            Repository = new ServiceRepository(new DataContext(user.Services));
            dataVM = new AuthenticationDataVM(Repository);
            DataContext = dataVM;          
        }
        private void InitializePasswordComplexityRectangles()
        {
            _passwordComplexityRectangles[0] = VeryWeakPasswordRectangle;
            _passwordComplexityRectangles[1] = WeakPasswordRectangle;
            _passwordComplexityRectangles[2] = NormalPasswordRectangle;
            _passwordComplexityRectangles[3] = StrongPasswordRectangle;
            _passwordComplexityRectangles[4] = VeryStrongPasswordRectangle;
        }

        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {

            FormHeader.Text = Properties.Resources.AddFormHeader;
            Animation.FormOpeningAnimation(AddEditForm, ShadowEffectHomePage);
            //FormOpeningAnimation(AddEditForm);

        }
       
        private void CloseForm_Click(object sender, RoutedEventArgs e)
        {
            Grid form = (Grid)((Button)sender).Parent;
            Animation.FormClosingAnimation(form, ShadowEffectHomePage);
            ClearForm();

        }


        private void HiddenPasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
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

        private void HiddenPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
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

        private string GeneratePassword()
        {
            return Membership.GeneratePassword(_lengthGeneratePassword, _lengthGeneratePassword / 4);
        }
        private void SuggestPassword_Click(object sender, RoutedEventArgs e)
        {
            HiddenPasswordTextBox.Text = GeneratePassword(); 
        }


        private void AddEditFormSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ResourceTextBox.Text) && !string.IsNullOrEmpty(LoginTextBox.Text) && !string.IsNullOrEmpty(HiddenPasswordTextBox.Text))
            {
                if (string.IsNullOrEmpty(IdTextBox.Text))
                {
                    Service service = new Service(ResourceTextBox.Text.ToLower(), LoginTextBox.Text, HiddenPasswordTextBox.Text);

                    Repository.Add(service);

                    Animation.FormClosingAnimation(AddEditForm, ShadowEffectHomePage);
                    ShowNotification(Properties.Resources.RecordAddedNotification);
                }
                else
                {
                    int ServiceId = int.Parse(IdTextBox.Text);     
                    Service service = new Service(ResourceTextBox.Text.ToLower(), LoginTextBox.Text, HiddenPasswordTextBox.Text);
                    Repository.Update(ServiceId, service);

                    Animation.FormClosingAnimation(AddEditForm, ShadowEffectHomePage);
                    ShowNotification(Properties.Resources.RecordEditedNotification);
                }
                ClearForm();
                dataVM.AuthenticationDataCollectionView.Refresh();
            }
            else
            {
                AddEditFormErrorTextBlock.Visibility = Visibility.Visible;
                AddEditFormErrorTextBlock.Text = Properties.Resources.FormFillAllFields;               
            }
        }
        private void ClearForm()
        {
            ResourceTextBox.Text = string.Empty;
            LoginTextBox.Text = string.Empty;
            HiddenPasswordTextBox.Text = string.Empty;
        }
        private Storyboard sb = new Storyboard();
        private void ShowNotification(string NotificationMessage)
        {
            sb.Completed -= Notification_Completed;
            // sb.Stop();

            NotificationBody.Visibility = Visibility.Visible;

            NotificationText.Text = NotificationMessage;



            DoubleAnimation notificationAppearingTranslate = new DoubleAnimation()
            {
                From = 0,
                To = -30,
                Duration = TimeSpan.FromSeconds(0.3),
                AccelerationRatio = 0.5

            };            DoubleAnimation notificationAppearingOpacity = new DoubleAnimation()
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
        private void RevealPassword_Checked(object sender, RoutedEventArgs e)
        {
            Grid parent = ((Grid)((ToggleButton)sender).Parent);
            TextBlock RevealedPasswordTextBlock = (TextBlock)parent.FindName("RevealedPasswordTextBlock");
            TextBlock HiddenPasswordTextBlock = (TextBlock)parent.FindName("HiddenPasswordTextBlock");
            RevealedPasswordTextBlock.Visibility = Visibility.Visible;
            HiddenPasswordTextBlock.Visibility = Visibility.Collapsed;

        }
        private void RevealPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            Grid parent = ((Grid)((ToggleButton)sender).Parent);
            TextBlock RevealedPasswordTextBlock = (TextBlock)parent.FindName("RevealedPasswordTextBlock");
            TextBlock HiddenPasswordTextBlock = (TextBlock)parent.FindName("HiddenPasswordTextBlock");
            RevealedPasswordTextBlock.Visibility = Visibility.Collapsed;
            HiddenPasswordTextBlock.Visibility = Visibility.Visible;
        }
        private void CopyCellText(object sender, MouseButtonEventArgs e)
        {
            //DataGridCell cell = (DataGridCell)sender;
            //string cellText;
            //int row = MainDataGrid.Items.IndexOf(cell);

            //if (((DataGridCell)sender).Column.DisplayIndex == 2)
            //{
            //    cellText = ((AuthenticationData)cell.DataContext).Password;

            //}
            //else
            //{
            //    cellText = ((TextBlock)cell.Content).Text;
            //}
            //Clipboard.SetText(cellText);
            //ShowNotification(Properties.Resources.TextCopiedToClipboardNotification);
        }

        private void PopupMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = (StackPanel)((Button)sender).Parent;
            Popup pp = (Popup)parent.FindName("PopupMenu");
            pp.IsOpen = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var popupEl = (Popup)((Border)((StackPanel)(((Button)sender).Parent)).Parent).Parent;
            popupEl.IsOpen = false;
            FormHeader.Text = Properties.Resources.EditFormHeader;

            Service item = (Service)MainDataGrid.CurrentItem;
            ResourceTextBox.Text = item.Name;
            LoginTextBox.Text = item.Login;
            HiddenPasswordTextBox.Text = item.Password;
            IdTextBox.Text = item.Id.ToString();

            Animation.FormOpeningAnimation(AddEditForm, ShadowEffectHomePage);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var popupEl = (Popup)((Border)((StackPanel)((Button)sender).Parent).Parent).Parent;
            popupEl.IsOpen = false;

            Animation.FormOpeningAnimation(DeleteForm, ShadowEffectHomePage);
        }
        private void ConfirmDeleatingButton_Click(object sender, RoutedEventArgs e)
        {
            Animation.FormClosingAnimation(DeleteForm, ShadowEffectHomePage);

            Repository.Remove((Service)MainDataGrid.CurrentItem);
            dataVM.AuthenticationDataCollectionView.Refresh();

            ShowNotification(Properties.Resources.RecordRemovedNotification);
        }


        private void HiddenPasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetPasswordComplexity();
            SetSuggestPasswordButton();
        }
        private void SetPasswordComplexity()
        {
            PasswordsService.PasswordScore passwordScore = PasswordsService.GetPasswordStrength(Password);

            SolidColorBrush solidColorBrush;
            string message = "";
            switch (passwordScore)
            {
                case PasswordsService.PasswordScore.VeryWeak:
                    solidColorBrush = new SolidColorBrush(_veryWeakPasswordRectangleColor);
                    message = Properties.Resources.PswdVeryWeak;
                    break;
                case PasswordsService.PasswordScore.Weak:
                    solidColorBrush = new SolidColorBrush(_weakPasswordRectangleColor);
                    message = Properties.Resources.PswdWeak;
                    break;
                case PasswordsService.PasswordScore.Medium:
                    solidColorBrush = new SolidColorBrush(_normalPasswordRectangleColor);
                    message = Properties.Resources.PswdNormal;
                    break;
                case PasswordsService.PasswordScore.Strong:
                    solidColorBrush = new SolidColorBrush(_strongPasswordRectangleColor);
                    message = Properties.Resources.PswdStrong;
                    break;
                case PasswordsService.PasswordScore.VeryStrong:
                    solidColorBrush = new SolidColorBrush(_veryStrongPasswordRectangleColor);
                    message = Properties.Resources.PswdVeryStrong;
                    break;
                default:
                    solidColorBrush = new SolidColorBrush(_nullPasswordRectangleColor);
                    break;
            }

            SetPasswordComplexityText(message, solidColorBrush);
            PasswordsService.SetPasswordComplexityRectangles(solidColorBrush, _nullPasswordRectangleColor, passwordScore, _passwordComplexityRectangles);
        }
        private void SetPasswordComplexityText(string message, SolidColorBrush colorBrush)
        {
            PasswordComplexityText = message;
            PasswordComplexityTextColor = colorBrush;
        }

        private void SetSuggestPasswordButton()
        {
            if (!string.IsNullOrEmpty(Password)) SuggestPassword.Visibility = Visibility.Hidden;
            else SuggestPassword.Visibility = Visibility.Visible;
        }
    }
}
