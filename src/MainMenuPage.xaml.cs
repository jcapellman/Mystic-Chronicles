using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MysticChronicles
{
    public sealed partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            this.InitializeComponent();
        }

        private async void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
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

            if (result == ContentDialogResult.Primary)
            {
                string heroName = string.IsNullOrWhiteSpace(textBox.Text) ? "Snake" : textBox.Text.Trim();
                this.Frame.Navigate(typeof(GamePage), heroName);
            }
        }

        private async void BtnLoadGame_Click(object sender, RoutedEventArgs e)
        {
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
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
