using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MysticChronicles
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainMenuPage), e.Arguments);
                }

                // Enter full screen mode and hide cursor globally for entire game session
                EnterFullScreenMode();
                HideCursor();

                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Enter full screen mode for immersive RPG experience
        /// </summary>
        private void EnterFullScreenMode()
        {
            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }

        /// <summary>
        /// Hide system cursor for entire game session (keyboard/gamepad only)
        /// </summary>
        private void HideCursor()
        {
            Window.Current.CoreWindow.PointerCursor = null;
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
