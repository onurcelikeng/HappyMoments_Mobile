using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View.Hamburger
{
    public sealed partial class ContactPage : Page
    {

        public ContactPage()
        {
            this.InitializeComponent();
            this.DrawerLayout.InitializeDrawerLayout();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void hamburgerButton_Click(object sender, RoutedEventArgs e)
        {
           if(DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
            }

           else
            {
                DrawerLayout.OpenDrawer();
            }
        }

        #region Back Button

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        #endregion

        #region Social Media 

        private async void facebook_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://facebook.com/"));
        }

        private async void twitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://twitter.com/"));
        }

        private async void youtube_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://youtube.com/"));
        }

        #endregion

        #region Hamburger Buttons

        private void homeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void sssButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Frame.Navigate(typeof(View.Hamburger.SSSPage));
        }

        private void settingsButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Frame.Navigate(typeof(View.Hamburger.NotificationPage));
        }

        private void cardButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();

            Frame.Navigate(typeof(View.Hamburger.CreditCardsPage));
        }

        private void logOutButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Application.Current.Exit();
        }

        private void howToButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void contactButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Frame.Navigate(typeof(View.Hamburger.ContactPage));
        }

        #endregion
    }
}