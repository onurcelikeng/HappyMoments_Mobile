using System;
using Windows.Storage;
using Windows.UI.Xaml;
using HappyMoments.View;
using HappyMoments.Helpers;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.ApplicationModel.Activation;
using HappyMoments.Client;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;

namespace HappyMoments
{
    public sealed partial class App : Application
    {
        public static string NotificationHubName = "happymoments", NotificationKey = "Endpoint=sb://happymoments.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=dmCV98kcWUoFjfac7qO/Kf151zuVFa50TG81tc4GpE8=";
        private ContinuationManager _continuator = new ContinuationManager();
        private static DataClient _client = new DataClient();
        private TransitionCollection transitions;


        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }
        

        public static DataClient Client
        {
            get
            {
                return _client;
            }
        }

        public static string GetDeviceId()
        {
            var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
            var hardwareId = token.Id;
            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

            byte[] bytes = new byte[hardwareId.Length];
            dataReader.ReadBytes(bytes);

            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private void CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                Window.Current.Content = rootFrame;
            }
        }

        protected async override void OnActivated(IActivatedEventArgs args)
        {
            CreateRootFrame();

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch { }
            }

            if (args is IContinuationActivatedEventArgs)
            {
                _continuator.ContinueWith(args);
            }

            Window.Current.Activate();
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.CacheSize = 1;

                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {

                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }
                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                object token = ApplicationData.Current.LocalSettings.Values["Token"];
                object pin = ApplicationData.Current.LocalSettings.Values["Pin"];

                if (token == null)
                {
                    rootFrame.Navigate(typeof(View.LoginPage));
                }

                else if (pin == null)
                {
                    rootFrame.Navigate(typeof(LoginPage));
                }

                else
                {
                    Client.SetToken(token.ToString());
                    var meContext = await Client.GetMe();

                    if (meContext.IsSuccess)
                    {
                        ProfilePage.user = meContext.Data;

                        if (ProfilePage.user != null)
                        {
                            rootFrame.Navigate(typeof(MainPage));
                        }
                    }

                    else
                    {
                        ApplicationData.Current.LocalSettings.Values["Token"] = null;
                        ApplicationData.Current.LocalSettings.Values["Pin"] = null;
                        await new MessageDialog(meContext.Message).ShowAsync();
                        rootFrame.Navigate(typeof(LoginPage));
                    }
                }
            }

            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}