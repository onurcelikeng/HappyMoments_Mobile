using HappyMomentsAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View
{
    public sealed partial class MakeHappyPage : Page
    {
        public static PostModel friend;
        private CardModel selectedCard;
        private PostPayModel postPay;
        private string txtMessage;
        private bool isaddCard;
        private Audio audio;


        public MakeHappyPage()
        {
            this.InitializeComponent();
            this.CardList();
            this.Loaded += MakeHappyPage_Loaded;
        }


        private void MakeHappyPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtMessage = null;
            audio = new Audio();
            DataContext = friend;
            MoneyScale.ValueChanged += MoneyScale_ValueChanged;
            happyPersonCounter.Text = 10 + " TL";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private async Task Pay()
        {
            try
            {
                if (isaddCard == true)
                {
                    postPay.PostId = friend.PostId;
                    postPay.Price = Convert.ToUInt32(MoneyScale.Value.ToString());
                    postPay.TextMessage = txtMessage;

                    var payContext = await App.Client.PayPost(postPay, null);
                    if (payContext.IsSuccess)
                    {
                        CardList();

                        addNewCard.Visibility = Visibility.Visible;
                        addingCard.DataContext = null;
                        addingCard.Visibility = Visibility.Collapsed;
                    }

                    else
                    {
                        MsgHelper.Show(payContext.Message);
                        return;
                    }

                    var meContext = await App.Client.GetMe();
                    if (!meContext.IsSuccess)
                    {
                        MsgHelper.Show(meContext.Message);
                        return;
                    }
                    ProfilePage.user = meContext.Data;
                }

                else
                {
                    postPay = new PostPayModel()
                    {
                        PostId = friend.PostId,
                        CardId = Guid.Parse(selectedCard.CardId),
                        IsNewCard = false,
                        Price = Convert.ToUInt32(MoneyScale.Value.ToString()),
                        TextMessage = txtMessage
                    };

                    var resultContext = await App.Client.PayPost(postPay, null);
                    if (resultContext.IsSuccess)
                    {
                        string momentPaidId = resultContext.Data;
                        if (list.SelectedIndex != -1)
                        {
                            var selectedAudio = list.Items[list.SelectedIndex] as Audio;
                            if (selectedAudio.Buffer != null)
                            {
                                var data = await selectedAudio.GetBuffer();
                                var audioContext = await App.Client.SendVoiceMessage(momentPaidId, postPay.PostId.ToString(), data);

                                if (!audioContext.IsSuccess)
                                {
                                    MsgHelper.Show(audioContext.Message);
                                }

                                try
                                {
                                    list.Items.Clear();
                                }

                                catch (Exception)
                                {

                                }
                            }
                        }
                    }

                    var meContext = await App.Client.GetMe();
                    if (!meContext.IsSuccess)
                    {
                        MsgHelper.Show(meContext.Message);
                        return;
                    }
                    ProfilePage.user = meContext.Data;
                }
            }

            catch (Exception ex)
            {

            }
        }

        private async void CardList()
        {
            flipViewIn.ItemsSource = null;
            var cardsContext = await App.Client.GetMyCards();
            if (!cardsContext.IsSuccess)
            {
                MsgHelper.Show(cardsContext.Message);
                return;
            }
            var cards = cardsContext.Data;
            if (cards.Count == 0)
                ((FlipViewItem)flipViewOut.Items[0]).Visibility = Visibility.Collapsed;
            else
                ((FlipViewItem)flipViewOut.Items[0]).Visibility = Visibility.Visible;
            flipViewIn.ItemsSource = cards;
        }

        private async void makeHappy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (flipViewIn.Items.Count == 0 && addingCard.Visibility == Visibility.Collapsed)
            {
                await new MessageDialog("Ödeme yapabilmeniz için kredi kartı eklemeniz gerekmektedir.", "Bildirim").ShowAsync();
            }

            else if (flipViewOut.SelectedIndex == 1 && addingCard.Visibility == Visibility.Collapsed)
            {
                await new MessageDialog("Ödeme yapabilmeniz için kayıtlı kartlarınızdan birisini seçiniz.", "Bildirim").ShowAsync();
            }

            else
            {
                addPin.ShowAt(sender as FrameworkElement);
            }
        }

        private async void enterPin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values["Pin"].ToString() == pinTextBox.Text)
            {
                addPin.Hide();
                pinTextBox.Text = String.Empty;

                await Pay();
            }

            else
            {
                addPin.Hide();
                pinTextBox.Text = String.Empty;

                await new MessageDialog("Güvenlik kodunuzu yanlış girdiniz. Tekrar deneyiniz.", "Bildirim").ShowAsync();
            }
        }

        private void MoneyScale_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MoneyScale.Value >= 10)
            {
                happyPersonCounter.Text = MoneyScale.Value.ToString() + " TL";
            }

            int scale = (int)MoneyScale.Value;

            if (10 <= scale && scale < 70)
            {
                Money.Source = new BitmapImage(new Uri("ms-appx:///Assets/ProfilePageIcons/pic1.png"));
            }

            if (70 <= scale && scale < 130)
            {
                Money.Source = new BitmapImage(new Uri("ms-appx:///Assets/ProfilePageIcons/pic1.png"));
            }

            if (130 <= scale && scale < 190)
            {
                Money.Source = new BitmapImage(new Uri("ms-appx:///Assets/ProfilePageIcons/pic1.png"));
            }

            if (190 <= scale && scale < 250)
            {
                Money.Source = new BitmapImage(new Uri("ms-appx:///Assets/ProfilePageIcons/pic1.png"));
            }
        }

        private async void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list.SelectedIndex != -1)
            {
                var audio = list.Items[list.SelectedIndex] as Audio;

                await audio.Play(Dispatcher);
            }
        }

        #region Flip View 

        private void flipViewIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCard = flipViewIn.SelectedItem as CardModel;
        }

        private void flipViewOut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flipViewOut != null)
            {
                int index = flipViewOut.SelectedIndex;

                if (index == 0)
                {
                    isaddCard = false;
                    postPay = null;
                }

                else if (index == 1)
                {
                    isaddCard = true;
                }
            }
        }

        #endregion

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

        #region Text Message

        private void sendMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtMessage = string.Empty;
            textMessage.Text = string.Empty;

            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void sendTextMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtMessage = textMessage.Text;
            textMessage.Text = string.Empty;
            message.Hide();
        }

        private void exitTextMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtMessage = string.Empty;
            textMessage.Text = string.Empty;
            message.Hide();
        }

        #endregion

        #region Voice Message 

        private void recordVoice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void exitVoiceMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            voiceMessage.Hide();
        }

        private void play_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Audio.Recording)
            {
                voiceText.Text = "Kayıt tamamlandı.";
                play.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icons/play.png"));
                audio.Stop();

                header.Opacity = 1;
                list.Items.Add(audio);
                audio = new Audio();
            }

            else
            {
                voiceText.Text = "Kayıt başladı.";
                play.Source = new BitmapImage(new Uri("ms-appx:///Assets/Icons/pause.png"));
                audio.Record();
            }
        }

        #endregion

        #region Add New Card

        private void addNewCard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (addingCard.Visibility == Visibility.Collapsed)
            {
                FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            }
        }

        private async void addCardButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CardName.Text) && !string.IsNullOrEmpty(CardNumber.Text) && !string.IsNullOrEmpty(CvcCode.Text) &&
                !string.IsNullOrEmpty(Month.Text) && !string.IsNullOrEmpty(Year.Text) && CardNumber.Text.Length == 16 &&
                Month.Text.Length == 2 && Year.Text.Length == 2 && CvcCode.Text.Length == 3)
            {
                isaddCard = true;

                postPay = new PostPayModel()
                {
                    CardName = CardName.Text,
                    CardNumber = CardNumber.Text,
                    CvcCode = CvcCode.Text,
                    Month = Month.Text,
                    Year = "20" + Year.Text,
                    IsNewCard = true
                };

                addNewCard.Visibility = Visibility.Collapsed;
                addingCard.DataContext = postPay;
                addingCard.Visibility = Visibility.Visible;

                var newMessage = new MessageDialog("Kredi kartınız başarılı br şekilde kaydedildi.", "Bildirim");
                newMessage.Commands.Add(new UICommand("Kapat"));
                IUICommand result = await newMessage.ShowAsync();
                if (result != null && result.Label == "Kapat") addCard.Hide();
            }

            else
            {
                isaddCard = false;
                await new MessageDialog("Lütfen kredi kartınızı tanımlarken eksiksiz bilgi giriniz.", "Bildirim").ShowAsync();
            }
        }

        private void exitaddCard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            isaddCard = false;
            addCard.Hide();
        }

        #endregion
    }
}