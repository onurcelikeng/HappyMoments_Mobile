using System;
using HappyMomentsAPI.Models;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace HappyMoments.View
{
    public sealed partial class DetailsNotificationPage : Page
    {
        public static NotificationModel notification;


        public DetailsNotificationPage()
        {
            this.InitializeComponent();
            this.Loaded += DetailsNotificationPage_Loaded;
        }


        private void InitializeUI()
        {
            if (notification.IsText == true)
            {
                textPanel.Visibility = Visibility.Visible;
                textContent.Visibility = Visibility.Visible;
            }

            if (notification.IsText == false)
            {
                textPanel.Visibility = Visibility.Collapsed;
                textContent.Visibility = Visibility.Collapsed;
            }

            if (notification.IsVoice == true)
            {
                voicePanel.Visibility = Visibility.Visible;
                voiceContent.Visibility = Visibility.Visible;
            }

            if (notification.IsVoice == false)
            {
                voicePanel.Visibility = Visibility.Collapsed;
                voiceContent.Visibility = Visibility.Collapsed;
            }

            if (notification.IsThanks == false)
            {
                thanksIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/NotificationIcons/dislike.png"));
            }

            if (notification.IsThanks == true)
            {
                thanksIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/NotificationIcons/like.png"));
            }
        }

        private void DetailsNotificationPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeUI();
            DataContext = notification;
            ReadNotification();

            if (notification.IsThanks == false)
            {
                thanks.Text = "Teşekkür et!";
            }

            else
            {
                thanks.Text = "Teşekkür ettin!";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private async void ReadNotification()
        {
            try
            {
                await App.Client.GetReadNotification(DetailsNotificationPage.notification.NotificationId);
            }

            catch (Exception)
            {

            }
        }

        private async void thanksButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                thanks.Text = "Teşekkür ettin!";
                thanksIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/NotificationIcons/like.png"));

                await App.Client.Thanks(new ReadNotificationModel()
                {
                    NotificationId = DetailsNotificationPage.notification.NotificationId
                });

            }

            catch (Exception)
            {

            }
        }

        private void play_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                voiceMedia.Source = new Uri(notification.VoiceMessage);
                voiceMedia.Play();
            }

            catch (Exception)
            {

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
    }
}