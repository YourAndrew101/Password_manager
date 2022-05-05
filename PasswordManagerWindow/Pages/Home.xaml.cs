﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using PasswordManager.MainWindow;
using PasswordManagerWindow.Models;
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

        private void AddPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            FormHeader.Text = Properties.Resources.AddFormHeader;
            FormOpeninganimation();

        }

        private void FormOpeninganimation() { 
         AddEditForm.Visibility = Visibility.Visible;
            ShadowEffectHomePage.Visibility = Visibility.Visible;
            ((MainWindow)App.Current.MainWindow).Shadow.Visibility = Visibility.Visible;
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
           
            ((MainWindow)App.Current.MainWindow).Shadow.BeginAnimation(OpacityProperty, shadowAppearingAnimation);
            
        }
        private void CloseEditAddForm_Click(object sender, RoutedEventArgs e)
        {
            FormClosingAnimation();
            ClearForm();

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
            ((MainWindow)App.Current.MainWindow).Shadow.BeginAnimation(OpacityProperty, shadowDisappearingAnimation);
        }
        private void ShadowDisappearingAnimation_Completed(object sender, EventArgs e)
        {
            ShadowEffectHomePage.Visibility = Visibility.Collapsed;
            ((MainWindow)App.Current.MainWindow).Shadow.Visibility = Visibility.Collapsed;
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
            if (string.IsNullOrEmpty(IdTextBox.Text))
            {
                dataVM.AuthenticationDataViewModels.Add(new Models.AuthenticationData(ResourceTextBox.Text, LoginTextBox.Text, PasswordTextBox.Text));
                FormClosingAnimation();
                ShowNotification(Properties.Resources.RecordAddedNotification);
                ClearForm();
            }
            else
            { int index = int.Parse(IdTextBox.Text);
                dataVM.AuthenticationDataViewModels[index].Resource=ResourceTextBox.Text;
                dataVM.AuthenticationDataViewModels[index].Login = LoginTextBox.Text;
                dataVM.AuthenticationDataViewModels[index].Password = PasswordTextBox.Text;
                FormClosingAnimation();
                ShowNotification(Properties.Resources.RecordEditedNotification);
                ClearForm();
            }
        }
         private void ClearForm()
        {
            ResourceTextBox.Text = string.Empty;
            LoginTextBox.Text = string.Empty;
            PasswordTextBox.Text = string.Empty;
        }
       private Storyboard sb = new Storyboard();
        private void ShowNotification(string NotificationMessage)
        {
           sb.Completed-= Notification_Completed;
           // sb.Stop();

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
         private void CopyCellText(object sender, MouseButtonEventArgs e)
        {
            string cellText = ((TextBlock)((DataGridCell)sender).Content).Text;
            Clipboard.SetText(cellText);
            ShowNotification(Properties.Resources.TextCopiedToClipboardNotification);
        }

        private void PopupMenuButton_Click(object sender, RoutedEventArgs e)
        {
        var parent = (StackPanel)((Button)sender).Parent;
        Popup pp =  (Popup)parent.FindName("PopupMenu");
        pp.IsOpen = true;
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var popupEl = (Popup)((Border)(((StackPanel)(((Button)sender).Parent)).Parent)).Parent;
            popupEl.IsOpen = false;
            FormHeader.Text = Properties.Resources.EditFormHeader;
            int index = MainDataGrid.Items.IndexOf(MainDataGrid.CurrentItem);
            var item = (AuthenticationData)MainDataGrid.Items[index];
            ResourceTextBox.Text = item.Resource;
            LoginTextBox.Text = item.Login;
            PasswordTextBox.Text = item.Password;
            IdTextBox.Text = index.ToString();
            FormOpeninganimation();


        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int index = MainDataGrid.Items.IndexOf(MainDataGrid.CurrentItem);
            dataVM.AuthenticationDataViewModels.RemoveAt(index);
            ShowNotification(Properties.Resources.RecordRemovedNotification);
        }

      
    }
}