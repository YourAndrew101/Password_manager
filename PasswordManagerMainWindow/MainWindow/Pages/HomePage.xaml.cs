using System;
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
using Data.DataProviders.Products;
using PasswordManager.MainWindow;
using PasswordManager.MainWindow.Models;
using PasswordManager.MainWindow.ViewModels;
using UsersLibrary;
using UsersLibrary.Services;

namespace PasswordManager.MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private User User { get; set; }

        AuthenticationDataVM dataVM = new AuthenticationDataVM();


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
            FormOpeningAnimation(AddEditForm);

        }

        private void FormOpeningAnimation(Grid form)
        {
            form.Visibility = Visibility.Visible;
            ShadowEffectHomePage.Visibility = Visibility.Visible;
            ((MainWindow)App.Current.Windows[0]).Shadow.Visibility = Visibility.Visible;
            DoubleAnimation formScaleAnimation = new DoubleAnimation()
            {
                From = 1.2,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.2),
                AccelerationRatio = 0.5,
            };
            form.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, formScaleAnimation);
           form.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, formScaleAnimation);

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

            ((MainWindow)App.Current.Windows[0]).Shadow.BeginAnimation(OpacityProperty, shadowAppearingAnimation);

        }
        private void FormClosingAnimation(Grid form)
        {
            
            DoubleAnimation formScaleAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(0.2),
                AccelerationRatio = 0.5,
            };
            formScaleAnimation.Completed += (sender, e) => formDisapperingAnimation_Comleted(sender, e, form);
            form.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, formScaleAnimation);
            form.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, formScaleAnimation);

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
            ((MainWindow)App.Current.Windows[0]).Shadow.BeginAnimation(OpacityProperty, shadowDisappearingAnimation);
        }
        private void ShadowDisappearingAnimation_Completed(object sender, EventArgs e)
        {
            ShadowEffectHomePage.Visibility = Visibility.Collapsed;
            ((MainWindow)App.Current.Windows[0]).Shadow.Visibility = Visibility.Collapsed;
        }

        private void formDisapperingAnimation_Comleted(object sender, EventArgs e, Grid form)
        {
            form.Visibility = Visibility.Collapsed;

        }
        private void CloseForm_Click(object sender, RoutedEventArgs e)
        {
            Grid form =(Grid)((Button)sender).Parent;
            FormClosingAnimation(form);
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

        private void SuggestPassword_Click(object sender, RoutedEventArgs e)
        {
            HiddenPasswordTextBox.Text = "test password";
        }

        private void AddEditFormSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ResourceTextBox.Text) && !string.IsNullOrEmpty(LoginTextBox.Text) && !string.IsNullOrEmpty(HiddenPasswordTextBox.Text))
            {
                if (string.IsNullOrEmpty(IdTextBox.Text))
                {
                    dataVM.AuthenticationDataViewModels.Add(new Models.AuthenticationData(ResourceTextBox.Text.ToLower(), LoginTextBox.Text, HiddenPasswordTextBox.Text));
                   
                    FormClosingAnimation(AddEditForm);
                    ShowNotification(Properties.Resources.RecordAddedNotification);
                }
                else
                {
                    int index = int.Parse(IdTextBox.Text);
                    dataVM.AuthenticationDataViewModels[index].Resource = ResourceTextBox.Text.ToLower();
                    dataVM.AuthenticationDataViewModels[index].Login = LoginTextBox.Text;
                    dataVM.AuthenticationDataViewModels[index].Password = HiddenPasswordTextBox.Text;
                    FormClosingAnimation(AddEditForm);
                    ShowNotification(Properties.Resources.RecordEditedNotification);

                }
                ClearForm();
                dataVM.AuthenticationDataCollectionView.Refresh();
            }
            else
            {
                PasswordComplexityTextBlock.Text = Properties.Resources.FormFillAllFields;
                PasswordComplexityTextBlock.Foreground = new SolidColorBrush(Colors.Red);
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
            AuthenticationData item = (AuthenticationData)MainDataGrid.CurrentItem;
            int index = MainDataGrid.Items.IndexOf(item);
            ResourceTextBox.Text = item.Resource;
            LoginTextBox.Text = item.Login;
            HiddenPasswordTextBox.Text = item.Password;
            IdTextBox.Text = index.ToString();
            FormClosingAnimation(AddEditForm);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var popupEl = (Popup)((Border)((StackPanel)(((Button)sender).Parent)).Parent).Parent;
            popupEl.IsOpen = false;
            
            FormOpeningAnimation(DeleteForm);
           
        }

        private void ConfirmDeleatingButton_Click(object sender, RoutedEventArgs e)
        {
            FormClosingAnimation(DeleteForm);
            dataVM.AuthenticationDataViewModels.Remove((AuthenticationData)MainDataGrid.CurrentItem);
            ShowNotification(Properties.Resources.RecordRemovedNotification);
        }

    }
}
