using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.System;

namespace MysticChronicles
{
    public sealed partial class MainMenuPage : Page
    {
        private int menuSelection = 0;
        private const int MenuItemCount = 3;
        private bool isDialogOpen = false;

        public MainMenuPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            UpdateMenuCursor();
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = true;

            if (isDialogOpen)
            {
                return;
            }

            if (args.VirtualKey == VirtualKey.Up || args.VirtualKey == VirtualKey.W)
            {
                menuSelection--;
                if (menuSelection < 0)
                {
                    menuSelection = MenuItemCount - 1;
                }
                UpdateMenuCursor();
            }
            else if (args.VirtualKey == VirtualKey.Down || args.VirtualKey == VirtualKey.S)
            {
                menuSelection++;
                if (menuSelection >= MenuItemCount)
                {
                    menuSelection = 0;
                }
                UpdateMenuCursor();
            }
            else if (args.VirtualKey == VirtualKey.Enter || args.VirtualKey == VirtualKey.Space)
            {
                ExecuteMenuSelection();
            }
        }

        private void UpdateMenuCursor()
        {
            cursorNewGame.Visibility = menuSelection == 0 ? Visibility.Visible : Visibility.Collapsed;
            cursorLoadGame.Visibility = menuSelection == 1 ? Visibility.Visible : Visibility.Collapsed;
            cursorExit.Visibility = menuSelection == 2 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ExecuteMenuSelection()
        {
            switch (menuSelection)
            {
                case 0:
                    BtnNewGame_Click(null, null);
                    break;
                case 1:
                    BtnLoadGame_Click(null, null);
                    break;
                case 2:
                    BtnExit_Click(null, null);
                    break;
            }
        }

        private async void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (isDialogOpen) return;

            isDialogOpen = true;

            var dialog = new ContentDialog
            {
                Title = "Create Character",
                PrimaryButtonText = "Start Game",
                CloseButtonText = "Cancel"
            };

            var textBox = new TextBox
            {
                PlaceholderText = "Enter character name",
                Text = "Snake",
                MaxLength = 20
            };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock 
            { 
                Text = "Enter your hero's name:",
                Margin = new Thickness(0, 0, 0, 10)
            });
            panel.Children.Add(textBox);

            dialog.Content = panel;

            var result = await dialog.ShowAsync();
            isDialogOpen = false;

            if (result == ContentDialogResult.Primary)
            {
                string heroName = string.IsNullOrWhiteSpace(textBox.Text) ? "Snake" : textBox.Text.Trim();
                this.Frame.Navigate(typeof(GamePage), heroName);
            }
        }

        private async void BtnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;

            bool saveExists = await SaveGameManager.SaveExists();

            if (saveExists)
            {
                var saveData = await SaveGameManager.LoadGame();
                if (saveData != null)
                {
                    this.Frame.Navigate(typeof(GamePage), saveData);
                }
                else
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Load Failed",
                        Content = "Failed to load saved game.",
                        CloseButtonText = "OK"
                    };
                    await dialog.ShowAsync();
                    Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
                }
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "No Save Data",
                    Content = "No saved game found.",
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
