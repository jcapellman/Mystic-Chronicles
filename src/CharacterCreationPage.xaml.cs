using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.System;

namespace MysticChronicles
{
    public sealed partial class CharacterCreationPage : BasePage
    {
        private int selection = 0;
        private const int OptionCount = 2;

        public CharacterCreationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            UpdateCursor();
            txtHeroName.Focus(FocusState.Programmatic);
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = true;

            if (args.VirtualKey == VirtualKey.Left || args.VirtualKey == VirtualKey.A)
            {
                selection = 0;
                UpdateCursor();
            }
            else if (args.VirtualKey == VirtualKey.Right || args.VirtualKey == VirtualKey.D)
            {
                selection = 1;
                UpdateCursor();
            }
            else if (args.VirtualKey == VirtualKey.Enter || args.VirtualKey == VirtualKey.Space)
            {
                ExecuteSelection();
            }
        }

        private void UpdateCursor()
        {
            cursorConfirm.Visibility = selection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorCancel.Visibility = selection == 1 ? Visibility.Visible : Visibility.Collapsed;

            txtConfirm.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(selection == 0 ? Windows.UI.Colors.Yellow : Windows.UI.Colors.White);
            txtCancel.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(selection == 1 ? Windows.UI.Colors.Yellow : Windows.UI.Colors.White);
        }

        private void ExecuteSelection()
        {
            if (selection == 0)
            {
                string heroName = string.IsNullOrWhiteSpace(txtHeroName.Text) ? "Snake" : txtHeroName.Text.Trim();
                this.Frame.Navigate(typeof(GamePage), heroName);
            }
            else
            {
                this.Frame.Navigate(typeof(MainMenuPage));
            }
        }
    }
}
