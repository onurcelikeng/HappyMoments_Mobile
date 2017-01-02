using System;
using HappyMomentsAPI.Models;
using System.Collections.Generic;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

namespace HappyMoments.View
{
    public sealed partial class TokenPage : Page
    {
        private List<uint> tokenList = new List<uint>();
        public static CompanyModel company;
        private uint totalToken;


        public TokenPage()
        {
            this.InitializeComponent();
            this.Loaded += TokenPage_Loaded;
        }


        private async void TokenPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                totalToken = 0;
                DataContext = company;
                var companiesContext = await App.Client.CompanyCodes(company.CompanyId.ToString());
                if (!companiesContext.IsSuccess)
                {
                    MsgHelper.Show(companiesContext.Message);
                    return;
                }
                list.ItemsSource = companiesContext.Data;
            }

            catch(Exception)
            {

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

        private void add_Click(object sender, RoutedEventArgs e)
        {
            uint coin = Convert.ToUInt32(((AppBarButton)sender).DataContext);

            totalToken += coin;
            tokenList.Add(coin);
            total.Text = totalToken.ToString() + " TL hediye çeki seçtiniz.";
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            uint coin = Convert.ToUInt32(((AppBarButton)sender).DataContext);

            if (totalToken - coin >= 0 && tokenList.Contains(coin))
            {
                totalToken -= coin;
                tokenList.Remove(coin);
                total.Text = totalToken.ToString() + " TL hediye çeki seçtiniz.";
            }

            if (tokenList.Count == 0)
            {
                total.Text = "Hiçbir hediye çeki seçmediniz.";
            }
        }

        private async void confirmButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (totalToken == 0)
                {
                    await new MessageDialog("Herhangi bir hediye çeki oluşturmadınız.", "Bildirim").ShowAsync();
                }

                else if (ProfilePage.user.TotalMoney >= totalToken)
                {
                    var newMessage = new MessageDialog("Hediye kuponunuzu oluşturmak istediğinize emin misin?", "Bildirim");
                    newMessage.Commands.Add(new UICommand("Evet"));
                    newMessage.Commands.Add(new UICommand("Hayır"));
                    IUICommand result = await newMessage.ShowAsync();

                    if (result != null && result.Label == "Evet")
                    {
                        var giftCardsContext = await App.Client.BuyGiftcards(company.CompanyId, tokenList);
                        
                        if (giftCardsContext.IsSuccess)
                        {
                            var meContext = await App.Client.GetMe();
                            if (!meContext.IsSuccess)
                            {
                                MsgHelper.Show(meContext.Message);
                                return;
                            }
                            ProfilePage.user = meContext.Data;
                            Frame.Navigate(typeof(View.GiftWalletPage));
                            await new MessageDialog("Yeni çekiniz cüzdanınıza eklenmiştir.", "Tebrikler").ShowAsync();
                        }
                        else
                        {
                            MsgHelper.Show(giftCardsContext.Message);
                        }
                    }
                }

                else
                {
                    await new MessageDialog("Yeteri kadar Happy puanınız bulunmamaktadır.", "Bildirim").ShowAsync();
                }
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
