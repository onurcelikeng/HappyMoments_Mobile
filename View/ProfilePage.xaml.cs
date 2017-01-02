using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;
using HappyMomentsAPI.Models;

namespace HappyMoments.View
{
    public sealed partial class ProfilePage : Page
    {
        public static UserModel user;


        public ProfilePage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager dataManager = DataTransferManager.GetForCurrentView();
            dataManager.DataRequested += DataShareControl;
            DataContext = ProfilePage.user;
        }

        private void DataShareControl(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = "Happy Moments";
            e.Request.Data.Properties.Description = "Merhaba bu benim profilim";
            e.Request.Data.SetWebLink(new Uri("http://happymoments.me/" + ProfilePage.user.UserName));
        }

        private void share_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
    }
}