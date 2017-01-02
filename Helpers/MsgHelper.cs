using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace HappyMoments
{
    public class MsgHelper
    {
        public static async void Show(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }
    }
}
