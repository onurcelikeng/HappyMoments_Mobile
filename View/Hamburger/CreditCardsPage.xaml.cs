using HappyMoments.Client;
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
    public sealed partial class CreditCardsPage : Page
    {

        public CreditCardsPage()
        {
            this.InitializeComponent();
            this.DrawerLayout.InitializeDrawerLayout();
            this.Loaded += CreditCardsPage_Loaded;    
        }


        private async void CardList()
        {
            try
            {
                var cardsContext = await App.Client.GetMyCards();
                if (!cardsContext.IsSuccess)
                {
                    MsgHelper.Show(cardsContext.Message);
                    return;
                }
                List<CardModel> cardList = cardsContext.Data;

                if (cardList.Count == 0)
                {
                    noCard.Visibility = Visibility.Visible;
                }

                else
                {
                    noCard.Visibility = Visibility.Collapsed;
                    listView.ItemsSource = cardList;
                }
            }

            catch (Exception)
            {

            }
        }

        private void CreditCardsPage_Loaded(object sender, RoutedEventArgs e)
        {
            CardList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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

        private async void deleteCard_Click(object sender, RoutedEventArgs e)
        {
            var newMessage = new MessageDialog("Kartınızı silmek istidiğinize emin misiniz?", "Bildirim");

            newMessage.Commands.Add(new UICommand("Evet"));
            newMessage.Commands.Add(new UICommand("Hayır"));
            IUICommand result = await newMessage.ShowAsync();

            if (result != null && result.Label == "Evet")
            {
                var selectedCard = (CardModel)((Button)sender).DataContext;

                if (selectedCard != null)
                {
                    if ((await App.Client.DeleteCard(selectedCard.CardId)).IsSuccess)
                    {
                        listView.ItemsSource = null;
                        CardList();

                        await new MessageDialog("Kart listeniz güncellendi.", "Bildirim").ShowAsync();
                    }
                }
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