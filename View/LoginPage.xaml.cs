using System;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Facebook;
using HappyMoments.Helpers;
using Windows.UI.Popups;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Microsoft.WindowsAzure.Messaging;
using Windows.Networking.PushNotifications;
using Windows.UI.ViewManagement;

namespace HappyMoments.View
{
    public sealed partial class LoginPage : Page, IWebAuthenticationBrokerContinuable
    {
        private FaceBookHelper helper;
        private FacebookClient client;
        

        public LoginPage()
        {
            this.InitializeComponent();
            this.InitializeUI();
            helper = new FaceBookHelper();
            client = new FacebookClient();
        }


        private async void InitializeUI()
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            circle1.Opacity = 1;
            circle2.Opacity = 0.4;
            circle3.Opacity = 0.4;
        }

        private void facebookLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            helper.LoginAndContinue();
        }

        private void flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flipView != null)
            {
                if (flipView.SelectedItem == flipViewItem1)
                {
                    circle1.Opacity = 1;
                    circle2.Opacity = 0.4;
                    circle3.Opacity = 0.4;
                }

                if (flipView.SelectedItem == flipViewItem2)
                {
                    circle1.Opacity = 0.4;
                    circle2.Opacity = 1;
                    circle3.Opacity = 0.4;
                }

                if (flipView.SelectedItem == flipViewItem3)
                {
                    circle1.Opacity = 0.4;
                    circle2.Opacity = 0.4;
                    circle3.Opacity = 1;
                }
            }
        }

        public async void ContinueWithWebAuthenticationBroker(WebAuthenticationBrokerContinuationEventArgs args)
        {
            try
            {
                progress.IsIndeterminate = true;

                helper.ContinueAuthentication(args);

                if (helper.AccessToken != null)
                {
                    var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                    var hub = new NotificationHub(App.NotificationHubName, App.NotificationKey);
                    var result = await hub.RegisterNativeAsync(channel.Uri, new List<string>() { App.GetDeviceId() });

                    if (result.RegistrationId != null)
                    {
                        string deviceData = App.GetDeviceId() + "~" + result.RegistrationId + "~Windows";
                        var token = await App.Client.GetToken(helper.AccessToken, deviceData);

                        if (!string.IsNullOrEmpty(token))
                        {
                            App.Client.SetToken(token);

                            var meContext = await App.Client.GetMe();
                            if (!meContext.IsSuccess)
                            {
                                await new MessageDialog(meContext.Message).ShowAsync();
                                return;
                            }

                            ProfilePage.user = meContext.Data;
                            if (ProfilePage.user != null)
                            {
                                ApplicationData.Current.LocalSettings.Values.Add("Token", token);

                                PinPage contentDialog = new PinPage();
                                ContentDialogResult content = await contentDialog.ShowAsync();

                                Frame.Navigate(typeof(MainPage));
                            }
                        }

                    }
                }

                else
                {

                }

                progress.IsIndeterminate = false;
            }

            catch (Exception)
            {
                progress.IsIndeterminate = false;
            }
        }
    }
}