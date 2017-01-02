using System;
using HappyMoments.View;
using HappyMomentsAPI.Models;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments
{
    public sealed partial class MainPage : Page
    {
        private static List<NotificationModel> notificationList = new List<NotificationModel>();
        private bool notification;
        private bool hamburger;


        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeUI();

            hamburger = false;
            notification = false;
            iframe.Navigate(typeof(ProfilePage));
        }


        private async void InitializeUI()
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            DrawerLayout.InitializeDrawerLayout();
            leftPanel.Visibility = Visibility.Visible;
            rightPanel.Visibility = Visibility.Collapsed;

            Header.Text = "Profilim";
            userButton.Opacity = 1;
            calenderButton.Opacity = 0.5;
            giftButton.Opacity = 0.5;
            walletButton.Opacity = 0.5;
        }

        private async void Notifications()
        {
            try
            {
                notificationList = null;
                notificationList = (await App.Client.GetNotifications()).Data;

                for (int i = 0; i < notificationList.Count; i++)
                {
                    if (notificationList[i].IsRead == false)
                    {
                        notificationList[i].Background = "#d8ffa4";
                    }

                    else
                    {
                        notificationList[i].Background = "#ffffff";
                    }

                    if (notificationList[i].IsText == true)
                    {
                        notificationList[i].MesOpacity = "1";
                    }

                    if (notificationList[i].IsText == false)
                    {
                        notificationList[i].MesOpacity = "0.2";
                    }

                    if (notificationList[i].IsVoice == true)
                    {
                        notificationList[i].MicOpacity = "1";
                    }

                    if (notificationList[i].IsVoice == false)
                    {
                        notificationList[i].MicOpacity = "0.2";
                    }
                }

                rightPanel.ItemsSource = notificationList;
            }

            catch (Exception)
            {

            }
        }

        private void rightPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DetailsNotificationPage.notification = rightPanel.SelectedItem as NotificationModel;

            if (DetailsNotificationPage.notification != null)
            {
                for (int i = 0; i < notificationList.Count; i++)
                {
                    if(notificationList[i] == DetailsNotificationPage.notification)
                    {
                        notificationList[i].Background = "#ffffff";
                    }
                }
                rightPanel.ItemsSource = notificationList;

                DrawerLayout.CloseDrawer();
                iframe.Navigate(typeof(DetailsNotificationPage));
            }
        }

        #region Top Menu Button

        private void hamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ListFragment.FlowDirection = FlowDirection.LeftToRight;
                ListFragment.HorizontalAlignment = HorizontalAlignment.Left;
                leftPanel.Visibility = Visibility.Visible;
                rightPanel.Visibility = Visibility.Collapsed;

                if (hamburger == true)
                {
                    if (notification == false)
                    {
                        DrawerLayout.CloseDrawer();
                        hamburger = false;
                    }

                    else
                    {
                        DrawerLayout.CloseDrawer();
                        hamburger = false;
                        notification = false;
                    }
                }

                else
                {
                    if (notification == false)
                    {
                        DrawerLayout.OpenDrawer();
                        hamburger = true;
                    }

                    else
                    {
                        ListFragment.FlowDirection = FlowDirection.RightToLeft;
                        ListFragment.HorizontalAlignment = HorizontalAlignment.Right;
                        rightPanel.Visibility = Visibility.Visible;
                        leftPanel.Visibility = Visibility.Collapsed;

                        DrawerLayout.CloseDrawer();
                        notification = false;
                    }
                }
            }

            catch (Exception)
            {

            }
        }

        private void notificationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Notifications();

                ListFragment.FlowDirection = FlowDirection.RightToLeft;
                ListFragment.HorizontalAlignment = HorizontalAlignment.Right;
                rightPanel.Visibility = Visibility.Visible;
                leftPanel.Visibility = Visibility.Collapsed;

                if (notification == true)
                {
                    if (hamburger == false)
                    {
                        DrawerLayout.CloseDrawer();
                        notification = false;
                    }

                    else
                    {
                        DrawerLayout.CloseDrawer();
                        hamburger = false;
                        notification = false;
                    }
                }

                else
                {
                    if (hamburger == false)
                    {
                        DrawerLayout.OpenDrawer();
                        notification = true;
                    }

                    else
                    {
                        ListFragment.FlowDirection = FlowDirection.LeftToRight;
                        ListFragment.HorizontalAlignment = HorizontalAlignment.Left;
                        leftPanel.Visibility = Visibility.Visible;
                        rightPanel.Visibility = Visibility.Collapsed;

                        DrawerLayout.CloseDrawer();
                        hamburger = false;
                    }
                }
            }

            catch (Exception)
            {

            }
        }

        #endregion

        #region Bottom Menu Buttons

        private void userButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            iframe.Navigate(typeof(View.ProfilePage));

            Header.Text = "Profilim";
            userButton.Opacity = 1;
            calenderButton.Opacity = 0.5;
            giftButton.Opacity = 0.5;
            walletButton.Opacity = 0.5;
        }

        private void calenderButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            iframe.Navigate(typeof(View.TimelinePage));

            Header.Text = "Arkadaşlarım";
            userButton.Opacity = 0.5;
            calenderButton.Opacity = 1;
            giftButton.Opacity = 0.5;
            walletButton.Opacity = 0.5;
        }

        private void giftButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            iframe.Navigate(typeof(View.GiftCardPage));

            Header.Text = "Hediye Çekleri";
            userButton.Opacity = 0.5;
            calenderButton.Opacity = 0.5;
            giftButton.Opacity = 1;
            walletButton.Opacity = 0.5;
        }

        private void walletButton_Click(object sender, RoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            iframe.Navigate(typeof(View.GiftWalletPage));

            Header.Text = "Cüzdanım";
            userButton.Opacity = 0.5;
            calenderButton.Opacity = 0.5;
            giftButton.Opacity = 0.5;
            walletButton.Opacity = 1;
        }

        #endregion

        #region Hamburger Buttons

        private void homeButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();

            Frame.Navigate(typeof(MainPage));
        }

        private void cardButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();

            Frame.Navigate(typeof(View.Hamburger.CreditCardsPage));
        }

        private void howToButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void sssButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Frame.Navigate(typeof(View.Hamburger.SSSPage));
        }

        private void contactButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Frame.Navigate(typeof(View.Hamburger.ContactPage));
        }

        private void settingsButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();
            Frame.Navigate(typeof(View.Hamburger.NotificationPage));
        }

        private async void logOutButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();

            var newMessage = new MessageDialog("Uygulamadan çıkış yapmak istediğinize emin misiniz?", "Bildirim");
            newMessage.Commands.Add(new UICommand("Evet"));
            newMessage.Commands.Add(new UICommand("Hayır"));
            IUICommand result = await newMessage.ShowAsync();

            if (result != null && result.Label == "Evet")
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Token"] = null;
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Pin"] = null;

                Application.Current.Exit();
            }
        }

        #endregion
    }
}
