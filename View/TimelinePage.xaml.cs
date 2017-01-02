using HappyMomentsAPI.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HappyMoments.View
{
    public sealed partial class TimelinePage : Page
    {
        private List<PostModel> friendList = new List<PostModel>();
        

        public TimelinePage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }


        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var postsContext = await App.Client.GetPosts();
                if (!postsContext.IsSuccess)
                {
                    MsgHelper.Show(postsContext.Message);
                    return;
                }
                friendList = postsContext.Data;
                lst1.ItemsSource = friendList;
            }

            catch (Exception)
            {

            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeHappyPage.friend = friendList[lst1.SelectedIndex];
            Frame.Navigate(typeof(View.MakeHappyPage));
        }
    }
}