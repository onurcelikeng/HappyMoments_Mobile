using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View.Hamburger
{
    public sealed partial class SSSPage : Page
    {

        public SSSPage()
        {
            this.InitializeComponent();
            DrawerLayout.InitializeDrawerLayout();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void hamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrawerLayout.IsDrawerOpen)
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

        #region Panel Buttons

        private void g1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid1.Height == 0)
            {
                grid1.Height = 100;
            }

            else
            {
                grid1.Height = 0;
            }
        }

        private void g2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid2.Height == 0)
            {
                grid2.Height = 100;
            }

            else
            {
                grid2.Height = 0;
            }
        }

        private void g3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid3.Height == 0)
            {
                grid3.Height = 100;
            }

            else
            {
                grid3.Height = 0;
            }
        }

        private void g4_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid4.Height == 0)
            {
                grid4.Height = 100;
            }

            else
            {
                grid4.Height = 0;
            }
        }

        private void g5_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid5.Height == 0)
            {
                grid5.Height = 100;
            }

            else
            {
                grid5.Height = 0;
            }
        }

        private void g6_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid6.Height == 0)
            {
                grid6.Height = 100;
            }

            else
            {
                grid6.Height = 0;
            }
        }

        private void g7_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid7.Height == 0)
            {
                grid7.Height = 100;
            }

            else
            {
                grid7.Height = 0;
            }
        }

        private void g8_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid8.Height == 0)
            {
                grid8.Height = 100;
            }

            else
            {
                grid8.Height = 0;
            }
        }

        private void g9_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grid9.Height == 0)
            {
                grid9.Height = 100;
            }

            else
            {
                grid9.Height = 0;
            }
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
