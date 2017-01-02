using System;
using HappyMoments.Client;
using HappyMomentsAPI.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HappyMoments.View
{
    public sealed partial class GiftCardPage : Page
    {


        public GiftCardPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }


        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                happyPoint.Text = ProfilePage.user.TotalMoney.ToString();

                var giftcards = await App.Client.GetCompanies();
                if (!giftcards.IsSuccess)
                {
                    MsgHelper.Show(giftcards.Message);
                    return;
                }

                lst1.ItemsSource = giftcards.Data;
            }

            catch (Exception)
            {

            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TokenPage.company = lst1.SelectedItem as CompanyModel;

            Frame.Navigate(typeof(View.TokenPage));
        }
    }
}
