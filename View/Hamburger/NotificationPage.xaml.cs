using HappyMoments.View.ContentDia;
using HappyMomentsAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View.Hamburger
{
    public sealed partial class NotificationPage : Page
    {

        public NotificationPage()
        {
            this.InitializeComponent();
            DrawerLayout.InitializeDrawerLayout();
            this.Loaded += NotificationPage_Loaded;
        }


        private void NotificationPage_Loaded(object sender, RoutedEventArgs e)
        {
            toggleLive.IsOn = ProfilePage.user.LiveNotification;
            toggleWeekly.IsOn = ProfilePage.user.WeeklyNotification;
            toggleRandom.IsOn = ProfilePage.user.RandomNotification;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            HappyMomentsAPI.Models.SettingsModel model = new HappyMomentsAPI.Models.SettingsModel()
            {
                LiveNotification = toggleLive.IsOn,
                RandomNotification = toggleRandom.IsOn,
                WeeklyNotification = toggleWeekly.IsOn
            };
            var settingsContext = await App.Client.UpdateSettings(model);
            if (!settingsContext.IsSuccess)
            {
                MsgHelper.Show(settingsContext.Message);
                return;
            }
            ProfilePage.user.LiveNotification = model.LiveNotification;
            ProfilePage.user.WeeklyNotification = model.WeeklyNotification;
            ProfilePage.user.RandomNotification = model.RandomNotification;

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

        private async void changePin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UpdatePinPage contentDialog = new UpdatePinPage();
            ContentDialogResult content = await contentDialog.ShowAsync();
        }

        private async void freezeAccount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DrawerLayout.CloseDrawer();

            var newMessage = new MessageDialog("Hesabınızı dondurmak istediğinize misiniz?", "Bildirim");
            newMessage.Commands.Add(new UICommand("Evet"));
            newMessage.Commands.Add(new UICommand("Hayır"));
            IUICommand result = await newMessage.ShowAsync();

            if (result != null && result.Label == "Evet")
            {
                SettingsModel model = new HappyMomentsAPI.Models.SettingsModel()
                {
                    LiveNotification = ProfilePage.user.LiveNotification,
                    WeeklyNotification = ProfilePage.user.WeeklyNotification,
                    RandomNotification = ProfilePage.user.RandomNotification,
                    IsFreeze = true
                };

                var updateContext = await App.Client.UpdateSettings(model);
                if (!updateContext.IsSuccess)
                {
                    MsgHelper.Show(updateContext.Message);
                    return;
                }

                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Token"] = null;
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["Pin"] = null;

                Application.Current.Exit();
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
