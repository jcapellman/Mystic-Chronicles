using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using GORE.Pages;
using GORE.Services;

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
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Activate window BEFORE navigation (required for Win2D)
                    Window.Current.Activate();

                    try
                    {
                        rootFrame.Navigate(typeof(MainMenuPage), e.Arguments);

                        // TEMPORARILY DISABLED to isolate the issue
                        // GoreEngine.ApplyGameMode();
                    }
                    catch (Exception ex)
                    {
                        // Log the full exception details
                        System.Diagnostics.Debug.WriteLine($"Navigation exception: {ex}");
                        System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException}");
                        throw;
                    }
                }
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            GoreEngine.Shutdown();
            deferral.Complete();
        }
    }
}
