using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View.ContentDia
{
    public sealed partial class UpdatePinPage : ContentDialog
    {

        public UpdatePinPage()
        {
            this.InitializeComponent();
        }


        private async void save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(oldPin.Text) && !string.IsNullOrEmpty(newPin.Text) && !string.IsNullOrEmpty(reEnterNewPin.Text))
            {
                if (newPin.Text == reEnterNewPin.Text && newPin.Text != oldPin.Text)
                {
                    ApplicationData.Current.LocalSettings.Values["Pin"] = null;
                    ApplicationData.Current.LocalSettings.Values["Pin"] = newPin.Text;

                    await new MessageDialog("Pininiz değiştirildi.", "Bildirim").ShowAsync();
                }

                else if (newPin.Text == oldPin.Text)
                {
                    await new MessageDialog("Yeni pin ile eski pin aynı. Lütfen farklı pin belirleyiniz.", "Uyarı").ShowAsync();
                }

                else if (newPin.Text != reEnterNewPin.Text)
                {
                    await new MessageDialog("Belirlediğiniz pin birbiri ile uyuşmuyor.", "Uyarı").ShowAsync();
                }
            }

            else
            {
                await new MessageDialog("Lütfen eksik bilgi girmeyiniz.", "Uyarı").ShowAsync();
            }
        }
    }
}
