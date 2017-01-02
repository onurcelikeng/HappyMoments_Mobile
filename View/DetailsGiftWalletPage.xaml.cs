using HappyMomentsAPI.Models;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View
{
    public sealed partial class DetailsGiftWalletPage : Page
    {
        public static MyGiftcardModel giftCard;


        public DetailsGiftWalletPage()
        {
            this.InitializeComponent();
            this.Loaded += DetailsGiftWalletPage_Loaded;
        }


        private void DetailsGiftWalletPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = giftCard;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
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
