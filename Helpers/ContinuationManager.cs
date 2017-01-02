using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HappyMoments.Helpers
{
    class ContinuationManager
    {
        public void ContinueWith(IActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            switch (args.Kind)
            {
                case ActivationKind.PickFileContinuation:
                    break;
                case ActivationKind.PickFolderContinuation:
                    break;
                case ActivationKind.PickSaveFileContinuation:
                    break;
                case ActivationKind.WebAuthenticationBrokerContinuation:
                    var continuator = rootFrame.Content as IWebAuthenticationBrokerContinuable;
                    if (continuator != null)
                        continuator.ContinueWithWebAuthenticationBroker((WebAuthenticationBrokerContinuationEventArgs)args);
                    break;
                default:
                    break;
            }
        }
    }

    interface IWebAuthenticationBrokerContinuable
    {
        void ContinueWithWebAuthenticationBroker(WebAuthenticationBrokerContinuationEventArgs args);
    }
}
