using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MysticChronicles
{
    /// <summary>
    /// Base page class providing common functionality for all game pages.
    /// Handles full screen mode and cursor management globally.
    /// </summary>
    public abstract class BasePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // Ensure full screen and cursor hidden on every page navigation
            // (in case user exited full screen with Alt+Enter or similar)
            EnsureFullScreenMode();
            EnsureCursorHidden();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            // Cursor and fullscreen remain hidden/active when navigating between pages
        }

        private void EnsureFullScreenMode()
        {
            var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            if (!view.IsFullScreenMode)
            {
                view.TryEnterFullScreenMode();
            }
        }

        private void EnsureCursorHidden()
        {
            if (Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor != null)
            {
                Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = null;
            }
        }
    }
}
