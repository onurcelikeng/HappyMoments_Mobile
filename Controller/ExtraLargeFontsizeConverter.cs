using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Data;

namespace HappyMoments.Controller
{
    public class ExtraLargeFontsizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int size = 0;
            var rawpixelperview = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var pixel = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            if (pixel == 1.2) size = 16;
            else if (pixel == 1.6) size = 18;
            else if (pixel == 2) size = 15;
            else if (pixel == 2.2) size = 20;
            else if (pixel == 2.6) size = 16;
            else size = 18;

            return size + 9;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
