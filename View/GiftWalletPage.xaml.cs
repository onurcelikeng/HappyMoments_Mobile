using HappyMomentsAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View
{
    public sealed partial class GiftWalletPage : Page
    {

        public GiftWalletPage()
        {
            this.InitializeComponent();
            this.Loaded += GiftWalletPage_Loaded;
        }


        private async void GiftWalletPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var cardsContext = await App.Client.GetMyGiftcards();
                if (!cardsContext.IsSuccess)
                {
                    MsgHelper.Show(cardsContext.Message);
                    return;
                }
                List<MyGiftcardModel> cardList = cardsContext.Data;

                if (cardList.Count == 0)
                {
                    noCard.Visibility = Visibility.Visible;
                }

                else
                {
                    noCard.Visibility = Visibility.Collapsed;
                    list.ItemsSource = cardList;
                }
            }

            catch (Exception)
            {

            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DetailsGiftWalletPage.giftCard = list.SelectedItem as MyGiftcardModel;
            Frame.Navigate(typeof(View.DetailsGiftWalletPage));
        }
    }
}
