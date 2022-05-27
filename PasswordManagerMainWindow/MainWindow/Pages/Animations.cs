using System;
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
    class Animation :PageFunctionBase
    {
        public static void FormOpeningAnimation(Grid form, Grid pageShadow)
        {
            form.Visibility = Visibility.Visible;
           pageShadow.Visibility = Visibility.Visible;
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
           form.BeginAnimation(OpacityProperty, formOpacityAnimation);

            DoubleAnimation shadowAppearingAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 0,
                To = 0.75,
            };
            pageShadow.BeginAnimation(OpacityProperty, shadowAppearingAnimation);

            ((MainWindow)App.Current.Windows[0]).Shadow.BeginAnimation(OpacityProperty, shadowAppearingAnimation);

        }
        public static void FormClosingAnimation(Grid form, Grid pageShadow)
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
            form.BeginAnimation(OpacityProperty, formOpacityAnimation);

            DoubleAnimation shadowDisappearingAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.2),
                From = 0.75,
                To = 0,
            };
            shadowDisappearingAnimation.Completed += (sender, e) => ShadowDisappearingAnimation_Completed(sender, e, pageShadow);
            pageShadow.BeginAnimation(OpacityProperty, shadowDisappearingAnimation);
            ((MainWindow)App.Current.Windows[0]).Shadow.BeginAnimation(OpacityProperty, shadowDisappearingAnimation);
        }
        private static void ShadowDisappearingAnimation_Completed(object sender, EventArgs e, Grid pageShadow)
        {
           pageShadow.Visibility = Visibility.Collapsed;
            ((MainWindow)App.Current.Windows[0]).Shadow.Visibility = Visibility.Collapsed;
        }

        private static void formDisapperingAnimation_Comleted(object sender, EventArgs e, Grid form)
        {
            form.Visibility = Visibility.Collapsed;

        }
    }
}
