using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HappyMoments.View
{
    public sealed partial class PinPage : ContentDialog
    {

        public PinPage()
        {
            this.InitializeComponent();
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            if (!string.IsNullOrEmpty(pinTextBox.Text) && pinTextBox.Text.Length == 4)
            {
                ApplicationData.Current.LocalSettings.Values.Add("Pin", pinTextBox.Text);
            }

            else
            {
                args.Cancel = true;
            }
        }

    }
}
